using UnityEngine;

public class GuardStartCommand : ICommand
{
    private readonly PlayerController _player;

    public GuardStartCommand(PlayerController player)
    {
        _player = player;
    }

    public void Execute()
    {
        _player.StartGuard();
    }
}
