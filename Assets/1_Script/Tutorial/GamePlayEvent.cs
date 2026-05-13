using UnityEngine;

public enum GamePlayEventType
{
    PlayerMoved,
    PlayerAttacked,
    BranchCollected,
    RockCollected,
    WeaponCrafted,
    PlayerRolled,
    EnemyKilled
}

public class GamePlayEvent : MonoBehaviour
{
    public GamePlayEventType eventType;

    public int amount;

    public ItemSO craftedItem;

    public EnemyType enemyType;

    public GamePlayEvent(
        GamePlayEventType eventType,
        int amount = 1,
        ItemSO craftedItem = null,
        EnemyType enemyType = EnemyType.Slime)
    {
        this.eventType = eventType;
        this.amount = amount;
        this.craftedItem = craftedItem;
        this.enemyType = enemyType;
    }
}
