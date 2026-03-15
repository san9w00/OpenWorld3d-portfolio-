using System.Collections.Generic;
using Unity.VisualScripting;
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

    [Header("UI State")]
    private bool _isInventoryOpen = false;

    private bool isCanInput = true;
    private bool isRunning;

    public void SetInputEnabled(bool value)
    {
        isCanInput = value;
    }

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
        if(!_isInventoryOpen)
        {
            LookAround();
        }

        if (!isCanInput)
            return;

        HandleInput();
        ProcessCommands();
    }

    private void HandleInput()
    {
        isRunning = Input.GetKey(KeyCode.LeftShift);

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

        if(!_isInventoryOpen)
        {
            // [공격] 입력
            if (Input.GetMouseButtonDown(0))
            {
                _commandQueue.Enqueue(new AttackCommand(_playerController));
            }
        }

        // [구르기] 입력
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            _commandQueue.Enqueue(new RollCommand(_playerController, direction));
        }

        // [점프] 입력
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _commandQueue.Enqueue(new JumpCommand(_playerController));
        }

        // [이동 (걷기 / 달리기)] 입력   
        if (isRunning)
        {
            _commandQueue.Enqueue(new RunCommand(_playerController, direction));
        }
        else
        {
            _commandQueue.Enqueue(new MoveCommand(_playerController, direction)); // 이동입력을 커맨드데이터로 캡슐화하여 큐에 삽입

        }

        // [방패] 입력
        if (Input.GetMouseButtonDown(1))
        {
            _commandQueue.Enqueue(new GuardStartCommand(_playerController));
        }
        else if (Input.GetMouseButtonUp(1))
        {
            _commandQueue.Enqueue(new GuardStopCommand(_playerController));
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerEquipment equipment = _playerController.GetComponent<PlayerEquipment>();

            if (equipment != null && equipment.CurrentWeapon != null)
            {
                SwordHitbox hitbox = equipment.CurrentWeapon.GetComponent<SwordHitbox>();

                if (hitbox != null && hitbox.WeaponData != null && hitbox.WeaponData.skill != null)
                {
                    hitbox.WeaponData.skill.TryUse(gameObject);
                }
            }
        }
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

    public void SetInventoryState(bool isOpen)
    {
        _isInventoryOpen = isOpen;

        if (isOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
