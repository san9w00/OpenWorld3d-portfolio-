using UnityEngine;

public class AttackCommand : ICommand
{
    private readonly PlayerController _player;

    public AttackCommand(PlayerController player)
    {
        _player = player;
    }

    public void Execute()
    {
        _player.Attack();

        EventBus.Publish(
                new GamePlayEvent(
                GamePlayEventType.PlayerAttacked));
    }
}
