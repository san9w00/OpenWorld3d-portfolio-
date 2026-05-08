using System.Collections.Generic;
using UnityEngine;

public class CraftingListView : MonoBehaviour
{
    [Header("Database")]
    [SerializeField] private CraftingDatabaseSO database;

    [Header("UI")]
    [SerializeField] private Transform contentParent;
    [SerializeField] private CraftButtonUI buttonPrefab;

    private List<CraftButtonUI> buttons = new();

    private void Start()
    {
        GenerateButtons();
    }

    private void OnEnable()
    {
        EventBus.Subscribe<InventoryChangedEvent>(OnInventoryChanged);
        RefreshButtons();
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<InventoryChangedEvent>(
            OnInventoryChanged);
    }

    // 버튼 생성
    private void GenerateButtons()
    {
        foreach (var recipe in database.recipes)
        {
            CraftRecipeViewModel vm = new CraftRecipeViewModel(recipe);

            CraftButtonUI button = Instantiate(buttonPrefab, contentParent);

            button.Setup(vm);

            buttons.Add(button);
        }
    }

    private void OnInventoryChanged(InventoryChangedEvent evt)
    {
        RefreshButtons();
    }

    private void RefreshButtons()
    {
        foreach (var button in buttons)
        {
            button.Refresh();
        }
    }
}
