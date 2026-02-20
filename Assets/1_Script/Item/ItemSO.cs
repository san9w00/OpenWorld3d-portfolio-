using UnityEngine;

public enum ItemType
{
    Weapon,
    potion
}

[CreateAssetMenu(menuName = "Item/ItemData")]
public class ItemSO : ScriptableObject
{
    public int itemId;
    public string itemName;
    public string itemExplain;
    public Sprite itemIcon;
    public ItemType itemType;
    public int maxStack = 1;

    public virtual void Use()
    {
        Debug.Log(itemName + "»ç¿ëµÊ");
    }
}
