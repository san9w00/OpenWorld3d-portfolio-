using UnityEngine;

public class GuardStopCommand : ICommand
{
    private readonly PlayerController _player;

    public GuardStopCommand(PlayerController player)
    {
        _player = player;
    }

    public void Execute()
    {
        _player.StopGuard();
    }
}
