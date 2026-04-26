using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotUII : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private QuickSlot quickSlot;

    [SerializeField] private PlayerInventory inventory;

    private void Start()
    {
        if (quickSlot != null)
            quickSlot.OnQuickSlotChanged += Refresh;

        if (inventory != null)
            inventory.OnInventoryChanged += Refresh;

        Refresh();
    }

    void Refresh()
    {
        if (quickSlot == null)
        {
            Debug.LogError("QuickSlot ¿¬°á ¾ÈµÊ");
            return;
        }

        if (icon == null || quantityText == null)
        {
            Debug.LogError("UI ¿¬°á ¾ÈµÊ");
            return;
        }

        if (quickSlot.currentItem == null || quickSlot.currentItem.itemData == null)
        {
            icon.enabled = false;
            quantityText.text = "";
            return;
        }

        icon.enabled = true;
        icon.sprite = quickSlot.currentItem.itemData.itemIcon;

        quantityText.text = quickSlot.currentItem.quantity > 1 ? quickSlot.currentItem.quantity.ToString() : "";
    }
}
