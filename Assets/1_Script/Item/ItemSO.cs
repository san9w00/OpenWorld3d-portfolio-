using UnityEngine;

public enum ItemType
{
    Weapon,
    potion
}

public abstract class ItemSO : ScriptableObject
{
    public string itemName;
    public int price;
    public string itemExplain;
    public Sprite itemIcon;
    public int maxStack = 1;

    public ItemType itemType;

    public abstract void Use(GameObject player);
}
