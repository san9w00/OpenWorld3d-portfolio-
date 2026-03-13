using UnityEngine;

public class RunCommand : ICommand
{
    private readonly PlayerController _player;
    private readonly Vector3 _moveDirection;

    public RunCommand(PlayerController player, Vector3 direction)
    {
        _player = player;
        _moveDirection = direction;
    }

    public void Execute()
    {
        _player.Run(_moveDirection);
    }
}
