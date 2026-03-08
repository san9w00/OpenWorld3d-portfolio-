using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private int maxSlotCount = 30;

    public List<InventoryItem> items = new List<InventoryItem>();

    public Action OnInventoryChanged; // 인벤토리가 변경될 때 호출되는 이벤트
    public Action<ItemSO, int> OnItemAdded; // 아이템 흭득 이벤트

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
                Debug.Log("인벤토리가 가득 찼습니다.");
                return;
            }

            items.Add(new InventoryItem(item, amount));
        }

        Debug.Log($"{item.itemName} 획득 / 현재 개수: {GetItemCount(item)}");

        OnItemAdded?.Invoke(item, amount);
        OnInventoryChanged?.Invoke();
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

        OnInventoryChanged?.Invoke();
    }
}
