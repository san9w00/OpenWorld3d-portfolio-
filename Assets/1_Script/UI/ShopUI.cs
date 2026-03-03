using UnityEngine;

// Singleton UI for Shop

public class ShopUI : MonoBehaviour
{
    public static ShopUI Instance;

    [SerializeField] private GameObject shopPanel;
    [SerializeField] private ShopSlotUI[] slots;

    private InputHandler _inputHandler;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }       
    }

    private void Start()
    {
        _inputHandler = FindAnyObjectByType<InputHandler>();
        shopPanel.SetActive(false);
    }

    public void Open(ShopData data)
    {
        ToggleShopPanel();

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < data.items.Length && data.items[i] != null)
            {
                slots[i].SetItem(data.items[i]);
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    void ToggleShopPanel()
    {
        bool isOpen = !shopPanel.activeSelf;
        shopPanel.SetActive(isOpen);
        _inputHandler.SetInventoryState(isOpen);
    }

    public void Close()
    {
        shopPanel.SetActive(false);
        _inputHandler.SetInventoryState(false);
    }
}
