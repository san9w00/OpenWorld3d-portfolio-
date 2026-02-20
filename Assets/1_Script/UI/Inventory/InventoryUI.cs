using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private List<SlotUI> slots;

    private bool isOpen = false;

    void Start()
    {
        playerInventory.OnInventoryChanged += RefreshUI;
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
        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);
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
    }
}
