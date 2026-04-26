using System;
using UnityEngine;

public class QuickSlot : MonoBehaviour
{
    [SerializeField] private PlayerInventory inventory;

    public InventoryItem currentItem;
    public Action OnQuickSlotChanged;

    public void SetItem(InventoryItem item)
    {
        currentItem = item;
        OnQuickSlotChanged?.Invoke();
    }

    public void UseItem(GameObject player)
    {
        if (currentItem == null) return;

        if (currentItem.quantity <= 0)
        {
            Clear();
            return;
        }

        currentItem.itemData.Use(player);

        inventory.RemoveItem(currentItem.itemData, 1);

        if (currentItem.quantity <= 0)
        {
            Clear();
        }

        OnQuickSlotChanged?.Invoke();
    }

    public void Clear()
    {
        currentItem = null;
        OnQuickSlotChanged?.Invoke();
    }
}
