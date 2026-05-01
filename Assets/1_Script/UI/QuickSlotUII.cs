using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotUII : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI quantityText;

    private void OnEnable()
    {
        EventBus.Subscribe<QuickSlotChangedEvent>(OnQuickSlotChangedMsg);
        EventBus.Subscribe<InventoryChangedEvent>(OnInventoryChangedMsg);

        Refresh();
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<QuickSlotChangedEvent>(OnQuickSlotChangedMsg);
        EventBus.UnSubscribe<InventoryChangedEvent>(OnInventoryChangedMsg);
    }

    private void OnQuickSlotChangedMsg(QuickSlotChangedEvent evt) => Refresh();
    private void OnInventoryChangedMsg(InventoryChangedEvent evt) => Refresh();

    void Refresh()
    {
        if (QuickSlot.Instance == null)
        {
            Debug.LogError("QuickSlot ¿¬°á ¾ÈµÊ");
            return;
        }

        if (icon == null || quantityText == null)
        {
            Debug.LogError("UI ¿¬°á ¾ÈµÊ");
            return;
        }

        if (QuickSlot.Instance.currentItem == null || QuickSlot.Instance.currentItem.itemData == null)
        {
            icon.enabled = false;
            quantityText.text = "";
            return;
        }

        icon.enabled = true;
        icon.sprite = QuickSlot.Instance.currentItem.itemData.itemIcon;

        quantityText.text = QuickSlot.Instance.currentItem.quantity > 1 ? QuickSlot.Instance.currentItem.quantity.ToString() : "";
    }
}
