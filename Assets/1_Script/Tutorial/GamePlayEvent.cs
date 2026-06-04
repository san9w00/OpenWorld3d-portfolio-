using UnityEngine;

public enum GamePlayEventType
{
    PlayerMoved,
    InteractCampfire,
    Teleport,
    PlayerRolled,
    BranchCollected,
    RockCollected,
    WeaponCrafted,
    EnterDungeon,
}

public class GamePlayEvent
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
