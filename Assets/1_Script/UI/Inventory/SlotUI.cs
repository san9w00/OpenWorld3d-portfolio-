using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private GameObject equipMark; // 무기 장착 표시

    private InventoryItem currentItem;
    private InventoryUI inventoryUI;

    private void Awake()
    {
        inventoryUI = GetComponentInParent<InventoryUI>();

        ClearSlot();
    }

    public void ClearSlot()
    {
        currentItem = null;
        if (icon != null) icon.enabled = false;
        if (quantityText != null) quantityText.text = "";
        if (equipMark != null) equipMark.SetActive(false);
    }

    public void SetItem(InventoryItem item)
    {
        // 아이템이 null이면 즉시 비우고 종료
        if (item == null || item.itemData == null)
        {
            ClearSlot();
            return;
        }

        currentItem = item;

        // 아이콘 설정
        icon.enabled = true;
        icon.sprite = item.itemData.itemIcon;

        // 수량 표시
        quantityText.text = item.quantity > 1 ? item.quantity.ToString() : "";

        // 무기 장착 표시 업데이트
        if (inventoryUI != null)
        {
            equipMark.SetActive(inventoryUI.IsEquipped(item.itemData));
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentItem != null)
        {
            inventoryUI.SelectItem(currentItem);
        }
    }
}
