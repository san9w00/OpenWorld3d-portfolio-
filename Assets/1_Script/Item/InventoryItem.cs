using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public ItemSO itemData;
    public int quantity; // ¼ö·®

    public InventoryItem(ItemSO data, int amount)
    {
        itemData = data;
        quantity = amount;
    }
}
