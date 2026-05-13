using UnityEngine;

public class MoveCommand : ICommand
{
    private readonly PlayerController _player;
    private readonly Vector3 _moveDirection;

    private static bool hasMovedOnce = false; // 최적화용

    public MoveCommand(PlayerController player, Vector3 direction)
    {
        _player = player;
        _moveDirection = direction;
    }

    public void Execute()
    {
        _player.Move(_moveDirection);

        // 실제 이동 입력이 잇을때
        if (_moveDirection != Vector3.zero && !hasMovedOnce)
        {
            hasMovedOnce = true;

            EventBus.Publish(
                new GamePlayEvent(
                GamePlayEventType.PlayerMoved));
        }
    }
}
