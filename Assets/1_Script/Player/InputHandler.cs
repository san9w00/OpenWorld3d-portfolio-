using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class InputHandler : MonoBehaviour
{
    private Queue<ICommand> _commandQueue = new Queue<ICommand>();

    [Header("Player Components")]
    private PlayerController _playerController;

    [Header("Camera Settings")]
    [SerializeField] private Transform cameraArm;
    [SerializeField] private float cameraSensitivity = 2f;
    [SerializeField] private float minVerticalAngle = -30f;
    [SerializeField] private float maxVerticalAngle = 70f;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        LookAround();

        HandleInput();
        ProcessCommands();
    }

    private void HandleInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 forward = cameraArm.forward;
        Vector3 right = cameraArm.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // (카메라 기준) 이동 방향
        Vector3 direction = (forward * v + right * h).normalized;

        // [공격] 입력
        if (Input.GetMouseButtonDown(0))
        {
            _commandQueue.Enqueue(new AttackCommand(_playerController));
        }

        // [구르기] 입력
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _commandQueue.Enqueue(new RollCommand(_playerController, direction));
        }

        // [점프] 입력
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _commandQueue.Enqueue(new JumpCommand(_playerController));
        }

        // [이동] 입력   
        _commandQueue.Enqueue(new MoveCommand(_playerController, direction)); // 이동입력을 커맨드데이터로 캡슐화하여 큐에 삽입
    }

    private void ProcessCommands()
    {
        // 큐에 쌓인 커맨드들을 순차적으로 실행
        while (_commandQueue.Count > 0)
        {
            ICommand command = _commandQueue.Dequeue();
            command.Execute();
        }
    }

    private void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector2 mouseDelta = new Vector2(mouseX, mouseY) * cameraSensitivity;

        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        // 각도 제한 로직 (기존과 동일)
        if (x < 180f)
        {
            x = Mathf.Clamp(x, minVerticalAngle, maxVerticalAngle);
        }
        else
        {
            x = Mathf.Clamp(x, 360f + minVerticalAngle, 360f + maxVerticalAngle);
        }

        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }
}
