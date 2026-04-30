using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// <Controller>
public class InventoryUI : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private List<SlotUI> slots;

    [Header("Detail UI")]
    [SerializeField] private GameObject detailPanel;
    [SerializeField] private Image detailIcon;
    [SerializeField] private TextMeshProUGUI detailName;
    [SerializeField] private TextMeshProUGUI detailDescription;

    private InventoryItem selectedItem;
    private WeaponItemSO curEquippedWeapon; // 현재 장착 무기;

    void Start()
    {
        PlayerInventory.Instance.OnInventoryChanged += RefreshUI;

        inventoryPanel.SetActive(false);

        ClearDatailUI();
        RefreshUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        bool isOpen = !inventoryPanel.activeSelf;
        inventoryPanel.SetActive(isOpen);

        InputHandler.Instance?.SetInventoryState(isOpen);

        if (!isOpen)
        {
            ClearDatailUI();
        }
    }

    void RefreshUI()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < PlayerInventory.Instance.items.Count)
            {
                slots[i].SetItem(PlayerInventory.Instance.items[i]);
            }
            else
            {
                slots[i].SetItem(null);
            }
        }

        if (selectedItem != null)
        {
            if (!PlayerInventory.Instance.items.Contains(selectedItem) || selectedItem.quantity <= 0)
            {
                selectedItem = null;
            }
        }
    }

    public void SelectItem(InventoryItem item)
    {
        selectedItem = item;

        ShowDetailUI(item);
        Debug.Log("선택됨: " + item.itemData.itemName);
    }

    void ShowDetailUI(InventoryItem item)
    {
        detailPanel.SetActive(true);
        detailIcon.sprite = item.itemData.itemIcon;
        detailName.text = item.itemData.itemName;
        detailDescription.text = item.itemData.itemExplain;
    }

    void ClearDatailUI()
    {
        detailPanel.SetActive(false);
        detailIcon.sprite = null;
        detailName.text = "";
        detailDescription.text = "";
    }

    public bool IsEquipped(ItemSO item)
    {
        if (curEquippedWeapon == null)
            return false;

        return curEquippedWeapon == item;
    }

    // 버튼용 메서드
    public void OnUseButton()
    {
        if (selectedItem == null)
        {
            Debug.Log("선택된 아이템이 없다.");
            return;
        }

        if (selectedItem.quantity <= 0)
        {
            Debug.Log("아이템 수량이 없다!");
            selectedItem = null;
            return;
        }

        if (selectedItem.itemData.itemType == ItemType.Weapon)
        {
            selectedItem.itemData.Use(PlayerInventory.Instance.gameObject);
            curEquippedWeapon = (WeaponItemSO)selectedItem.itemData;
            RefreshUI();
            return;
        }

        // 포션 -> 퀵슬롯 등록
        if (selectedItem.itemData.itemType == ItemType.potion)
        {
            if (QuickSlot.Instance == null)
            {
                Debug.LogError("QuickSlot 연결 안됨");
                return;
            }

            QuickSlot.Instance.SetItem(selectedItem);
        }
    }

    public void CloseButton()
    {
        inventoryPanel.SetActive(false);

        InputHandler.Instance?.SetInventoryState(false);

        ClearDatailUI();
    }

    private void OnDestroy()
    {
        if (PlayerInventory.Instance != null)
        {
            PlayerInventory.Instance.OnInventoryChanged -= RefreshUI;
        }
    }
}
