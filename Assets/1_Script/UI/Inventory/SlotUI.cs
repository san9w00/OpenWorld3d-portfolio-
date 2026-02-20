using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI quantityText;

    private InventoryItem currentItem;
    private InventoryUI inventoryUI;

    private void Awake()
    {
        inventoryUI = GetComponentInParent<InventoryUI>();
    }

    public void Initialize(InventoryUI ui)
    {
        inventoryUI = ui;
    }

    public void SetItem(InventoryItem item)
    {
        currentItem = item;

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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentItem != null)
        {
            inventoryUI.SelectItem(currentItem);
        }
    }
}
