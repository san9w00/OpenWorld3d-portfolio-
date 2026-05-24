using UnityEngine;

public class InventoryViewModel : MonoBehaviour
{
    private PlayerEquipment playerEquipment;

    private InventoryItem selectedItem;

    private void Awake()
    {
        playerEquipment = GetComponent<PlayerEquipment>();
    }

    private void OnEnable()
    {
        EventBus.Subscribe<InventorySelectionChangedEvent>(OnSelectionChanged);

        EventBus.Subscribe<QuickSlotChangedEvent>(OnQuickSlotChanged);

        EventBus.Subscribe<WeaponChangedEvent>(OnWeaponChanged);
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<InventorySelectionChangedEvent>(OnSelectionChanged);

        EventBus.UnSubscribe<QuickSlotChangedEvent>(OnQuickSlotChanged);

        EventBus.UnSubscribe<WeaponChangedEvent>(OnWeaponChanged);
    }

    private void OnSelectionChanged(InventorySelectionChangedEvent evt)
    {
        selectedItem = evt.Item;

        RefreshActionState();
    }

    private void OnQuickSlotChanged(QuickSlotChangedEvent evt)
    {
        RefreshActionState();
    }

    private void OnWeaponChanged(WeaponChangedEvent evt)
    {
        RefreshActionState();
    }

    private void RefreshActionState()
    {
        if (selectedItem == null)
        {
            EventBus.Publish(new InventoryActionStateChangedEvent(false, ""));

            return;
        }

        string buttonText = "";

        ItemSO item = selectedItem.itemData;

        // 무기
        if (item.itemType == ItemType.Weapon)
        {
            bool equipped =
                playerEquipment.GetCurrentWeaponData() == item;

            buttonText = equipped ? "Out" : "Equip";
        }
        // 포션
        else if (item.itemType == ItemType.Potion)
        {
            bool registered =
                QuickSlot.Instance.currentItem != null &&
                QuickSlot.Instance.currentItem.itemData == item;

            buttonText = registered ? "Out" : "Add";
        }

        EventBus.Publish(new InventoryActionStateChangedEvent(true, buttonText));
    }
}
