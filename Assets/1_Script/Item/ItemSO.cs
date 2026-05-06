using UnityEngine;

public enum ItemType
{
    Weapon,
    Potion,
    Resource,
}

public abstract class ItemSO : ScriptableObject
{
    [Header("Save")]
    public string itemID;

    public string itemName;
    public int price;
    public string itemExplain;
    public Sprite itemIcon;
    public int maxStack = 1;

    public ItemType itemType;

    public abstract void Use(GameObject player);
}
