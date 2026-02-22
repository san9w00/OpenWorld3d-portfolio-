using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// <Controller>
public class InventoryUI : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private List<SlotUI> slots;

    [Header("Detail UI")]
    [SerializeField] private GameObject detailPanel;
    [SerializeField] private Image detailIcon;
    [SerializeField] private TextMeshProUGUI detailName;
    [SerializeField] private TextMeshProUGUI detailDescription;

    private InputHandler _inputHandler;
    private InventoryItem selectedItem;
    private WeaponItemSO curEquippedWeapon; // 현재 장착 무기;

    void Start()
    {
        _inputHandler = FindAnyObjectByType<InputHandler>();
        inventoryPanel.SetActive(false);

        playerInventory.OnInventoryChanged += RefreshUI;

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

        _inputHandler.SetInventoryState(isOpen);

        if (!isOpen)
        {
            ClearDatailUI();
        }
    }

    void RefreshUI()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < playerInventory.items.Count)
            {
                slots[i].SetItem(playerInventory.items[i]);
            }
            else
            {
                slots[i].SetItem(null);
            }
        }

        if (selectedItem != null)
        {
            if (!playerInventory.items.Contains(selectedItem) || selectedItem.quantity <= 0)
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

        selectedItem.itemData.Use(playerInventory.gameObject);

        if (selectedItem.itemData.itemType == ItemType.Weapon)
        {
            curEquippedWeapon = (WeaponItemSO)selectedItem.itemData;
            RefreshUI();
        }

        // 포션 -> 소비
        if (selectedItem.itemData.itemType == ItemType.potion)
        {
            playerInventory.RemoveItem(selectedItem.itemData, 1);
        }
    }

    public void CloseButton()
    {
        inventoryPanel.SetActive(false);
        _inputHandler.SetInventoryState(false);
        ClearDatailUI();
    }
}
