using UnityEngine;

public class MoveCommand : ICommand
{
    private readonly PlayerController _player;
    private readonly Vector3 _moveDirection;

    public MoveCommand(PlayerController player, Vector3 direction)
    {
        _player = player;
        _moveDirection = direction;
    }

    public void Execute()
    {
        _player.Move(_moveDirection);
    }
}
