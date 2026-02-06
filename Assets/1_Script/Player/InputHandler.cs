using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private PlayerController _playerController;
    private Queue<ICommand> _commandQueue = new Queue<ICommand>();

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        HandleInput();
        ProcessCommands();
    }

    private void HandleInput()
    {
        // 입력감지
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(h, 0, v).normalized;

        // 이동입력을 커맨드데이터로 캡슐화하여 큐에 삽입
        ICommand moveCommand = new MoveCommand(_playerController, direction);
        _commandQueue.Enqueue(moveCommand);

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
}
