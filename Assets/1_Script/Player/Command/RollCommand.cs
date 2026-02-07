using UnityEngine;

public class RollCommand : ICommand
{
    private readonly PlayerController _player;
    private readonly Vector3 _rollDirection;

    public RollCommand(PlayerController player, Vector3 direction)
    {
        _player = player;
        _rollDirection = direction;
    }

    public void Execute()
    {
        _player.Roll(_rollDirection);
    }
}
