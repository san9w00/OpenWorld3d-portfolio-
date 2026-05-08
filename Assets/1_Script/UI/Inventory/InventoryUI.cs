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

    private InventoryItem selectedItem;
    private WeaponItemSO curEquippedWeapon; // 현재 장착 무기;

    private void OnEnable()
    {
        EventBus.Subscribe<InventoryChangedEvent>(OnInventoryChangedMsg);

        // 인벤토리가 켜질 때마다 최신화된 상태로 보여주기 위해 초기화
        if (PlayerInventory.Instance != null)
        {
            RefreshUI(PlayerInventory.Instance.items);
        }
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<InventoryChangedEvent>(OnInventoryChangedMsg);
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

        InputHandler.Instance?.SetInventoryState(isOpen);

        if (!isOpen)
        {
            ClearDatailUI();
        }
    }

    void RefreshUI(List<InventoryItem> currentItems)
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

            // UI 상태 즉시 갱신
            RefreshUI(PlayerInventory.Instance.items);
        }
        // 2. 포션일 경우
        else if (selectedItem.itemData.itemType == ItemType.Potion)
        {
            // [변경] 직접 QuickSlot 인스턴스를 찾는 대신 이벤트를 발행합니다.
            // 퀵슬롯 시스템은 이 이벤트를 듣고 있다가 스스로 아이템을 등록할 것입니다.
            EventBus.Publish(new QuickSlotChangedEvent(selectedItem));
        }

        // 공통: 아이템 사용 요청 이벤트 (로그나 퀘스트 시스템 등에서 활용 가능)
        EventBus.Publish(new ItemUseRequestedEvent(selectedItem));
    }

    public void CloseButton()
    {
        inventoryWindow.SetActive(false);

        InputHandler.Instance?.SetInventoryState(false);

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
