using System;
using UnityEngine;

public class QuickSlot : MonoBehaviour
{
    public static QuickSlot Instance;

    public InventoryItem currentItem;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        EventBus.Subscribe<QuickSlotChangedEvent>(OnQuickSlotChangeRequested);
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<QuickSlotChangedEvent>(OnQuickSlotChangeRequested);
    }

    // 이벤트가 발생했을 때 호출될 함수
    private void OnQuickSlotChangeRequested(QuickSlotChangedEvent evt)
    {
        this.currentItem = evt.CurrentItem;
    }

    public void SetItem(InventoryItem item)
    {
        currentItem = item;

        // UI 갱신을 위해 발행
        EventBus.Publish(new QuickSlotChangedEvent(currentItem));
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
        else
        {
            // 수량 변경 알림
            EventBus.Publish(new QuickSlotChangedEvent(currentItem));
        }
    }

    public void Clear()
    {
        currentItem = null;
        EventBus.Publish(new QuickSlotChangedEvent(null));
    }
}
