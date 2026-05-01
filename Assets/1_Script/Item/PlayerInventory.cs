using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    [SerializeField] private int maxSlotCount = 30;

    public List<InventoryItem> items = new List<InventoryItem>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddItem(ItemSO item, int amount = 1)
    {
        InventoryItem existing = items.Find(x => x.itemData == item);

        if (existing != null && existing.itemData.maxStack > 1)
        {
            existing.quantity += amount;
        }
        else
        {
            if (items.Count >= maxSlotCount)
            {
                Debug.Log("ÀÎº¥Åä¸®°¡ °¡µæ Ã¡½À´Ï´Ù.");
                return;
            }

            items.Add(new InventoryItem(item, amount));
        }

        Debug.Log($"{item.itemName} È¹µæ / ÇöÀç °³¼ö: {GetItemCount(item)}");

        EventBus.Publish(new ItemAddedEvent(item, amount)); // È¹µæ ¿¬Ãâ¿ë
        EventBus.Publish(new InventoryChangedEvent(items)); // ÀüÃ¼ UI °»½Å¿ë
    }

    public int GetItemCount(ItemSO item)
    {
        InventoryItem existing = items.Find(x => x.itemData == item);
        return existing != null ? existing.quantity : 0;
    }

    public void RemoveItem(ItemSO item, int amount = 1)
    {
        InventoryItem existing = items.Find(x => x.itemData == item);
        if (existing == null) return;

        existing.quantity -= amount;

        if (existing.quantity <= 0)
        {
            items.Remove(existing);
        }

        EventBus.Publish(new InventoryChangedEvent(items));
    }
}
