using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

// <Controller>
public class InventoryUI : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private GameObject inventoryWindow;
    [SerializeField] private List<SlotUI> slots;

    [Header("Tab panels")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject CraftPanel;

    [Header("Detail UI")]
    [SerializeField] private GameObject detailPanel;
    [SerializeField] private Image detailIcon;
    [SerializeField] private TextMeshProUGUI detailName;
    [SerializeField] private TextMeshProUGUI detailDescription;

    [Header("Action Button")]
    [SerializeField] private Button actionButton;
    [SerializeField] private TextMeshProUGUI actionButtonText;

    private InventoryItem selectedItem;

    private void OnEnable()
    {
        EventBus.Subscribe<InventoryChangedEvent>(OnInventoryChangedMsg);
        EventBus.Subscribe<InventoryActionStateChangedEvent>(OnActionStateChanged);

        // 인벤토리가 켜질 때마다 최신화된 상태로 보여주기 위해 초기화
        if (PlayerInventory.Instance != null)
        {
            RefreshUI(PlayerInventory.Instance.items);
        }
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<InventoryChangedEvent>(OnInventoryChangedMsg);
        EventBus.UnSubscribe<InventoryActionStateChangedEvent>(OnActionStateChanged);
    }

    void Start()
    {
        inventoryWindow.SetActive(false);
        ClearDatailUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventory();
        }
    }

    // 이벤트 매개변수를 처리하는 래퍼
    private void OnInventoryChangedMsg(InventoryChangedEvent evt)
    {
        RefreshUI(evt.Items);
    }

    void ToggleInventory()
    {
        bool isOpen = !inventoryWindow.activeSelf;
        inventoryWindow.SetActive(isOpen);

        if (isOpen)
        {
            UIManager.Instance.OpenUI(inventoryWindow);
        }
        else
        {
            UIManager.Instance.CloseUI(inventoryWindow);
            ClearDatailUI();
        }
    }

    void RefreshUI(List<InventoryItem> currentItems)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < currentItems.Count)
            {
                slots[i].SetItem(currentItems[i]);
            }
            else
            {
                slots[i].SetItem(null);
            }
        }

        if (selectedItem != null)
        {
            if (!currentItems.Contains(selectedItem) || selectedItem.quantity <= 0)
            {
                selectedItem = null;
            }
        }
    }

    public void SelectItem(InventoryItem item)
    {
        selectedItem = item;

        ShowDetailUI(item);

        EventBus.Publish(new InventorySelectionChangedEvent(item));
        Debug.Log("선택됨: " + item.itemData.itemName);
    }

    private void OnActionStateChanged(InventoryActionStateChangedEvent evt)
    {
        actionButton.gameObject.SetActive(evt.ShowButton);

        actionButtonText.text = evt.ButtonText;
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

    // 버튼용 메서드
    public void OnUseButton()
    {
        if (selectedItem == null)
            return;

        ItemSO item = selectedItem.itemData;

        // 무기
        if (item.itemType == ItemType.Weapon)
        {
            WeaponItemSO weapon = item as WeaponItemSO;

            PlayerEquipment equipment =
                PlayerInventory.Instance.GetComponent<PlayerEquipment>();

            bool equipped =
                equipment.GetCurrentWeaponData() == weapon;

            // 장착 해제
            if (equipped)
            {
                equipment.EquipWeapon(equipment.DefaultWeapon);
            }
            // 장착
            else
            {
                equipment.EquipWeapon(weapon);
            }
        }

        // 포션
        else if (item.itemType == ItemType.Potion)
        {
            bool registered =
                QuickSlot.Instance.currentItem != null &&
                QuickSlot.Instance.currentItem.itemData == item;

            // 해제
            if (registered)
            {
                QuickSlot.Instance.Clear();
            }
            // 등록
            else
            {
                QuickSlot.Instance.SetItem(selectedItem);
            }
        }

        // 버튼 상태 다시 갱신
        EventBus.Publish(
            new InventorySelectionChangedEvent(selectedItem)
        );
    }

    public void CloseButton()
    {
        inventoryWindow.SetActive(false);

        UIManager.Instance.CloseUI(inventoryWindow);

        ClearDatailUI();
    }

    public void OpenInventoryTab()
    {
        inventoryPanel.SetActive(true);
        CraftPanel.SetActive(false);
    }

    public void OpenCraftTab()
    {
        inventoryPanel.SetActive(false);
        CraftPanel.SetActive(true);
    }
}
