using System;
using UnityEngine;

public class QuickSlot : MonoBehaviour
{
    public static QuickSlot Instance;

    public InventoryItem currentItem;
    public Action OnQuickSlotChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

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

        PlayerInventory.Instance.RemoveItem(currentItem.itemData, 1);

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
