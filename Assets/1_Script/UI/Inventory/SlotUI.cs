using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI quantityText;

    public void SetItem(InventoryItem item)
    {
        if (item == null)
        {
            icon.enabled = false;
            quantityText.text = "";
            return;
        }

        icon.enabled = true;
        icon.sprite = item.itemData.itemIcon;

        quantityText.text = item.quantity > 1 ? item.quantity.ToString() : "";
    }
}
