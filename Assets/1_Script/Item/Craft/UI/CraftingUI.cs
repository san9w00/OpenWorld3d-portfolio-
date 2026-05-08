using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    [Header("Datail")]
    [SerializeField] private GameObject detailPanel;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI description;

    [Header("Resource UI")]
    [SerializeField] private Transform resourceParent;
    [SerializeField] private RequiredResourceUI resourcePrefab;
    private List<RequiredResourceUI> resourceUIs = new();

    [Header("Craft")]
    [SerializeField] private Button craftButton;

    private CraftRecipeViewModel currentVM;

    private void OnEnable()
    {
        EventBus.Subscribe<CraftRecipeSelectedEvent>(OnRecipeSelected);
        EventBus.Subscribe<InventoryChangedEvent>(OnInventoryChanged);

        if (currentVM != null)
        {
            Refresh();
        }
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<CraftRecipeSelectedEvent>(OnRecipeSelected);
        EventBus.UnSubscribe<InventoryChangedEvent>(OnInventoryChanged);
    }

    private void OnRecipeSelected(CraftRecipeSelectedEvent evt)
    {
        currentVM = evt.ViewModel;

        detailPanel.SetActive(true);

        Refresh();
    }

    private void OnInventoryChanged(InventoryChangedEvent evt)
    {
        if (currentVM == null) return;

        Refresh();
    }

    private void Start()
    {
        HideDetailPanel();
    }

    private void HideDetailPanel()
    {
        detailPanel.SetActive(false);
    }

    private void Refresh()
    {
        if (currentVM == null)
            return;

        itemIcon.sprite = currentVM.Icon;
        itemName.text = currentVM.ItemName;
        description.text = currentVM.Description;

        craftButton.interactable = currentVM.CanCraft;

        RefreshResources();
    }

    private void RefreshResources()
    {
        foreach (var ui in resourceUIs)
        {
            Destroy(ui.gameObject);
        }

        resourceUIs.Clear();

        foreach (var need in currentVM.Recipe.requiredResources)
        {
            RequiredResourceViewModel vm = new RequiredResourceViewModel(need);

            RequiredResourceUI ui = Instantiate(resourcePrefab, resourceParent);

            ui.Setup(vm);

            resourceUIs.Add(ui);
        }
    }

    public void OnClickCraft()
    {
        if (currentVM == null) return;

        EventBus.Publish(new CraftRequestEvent(currentVM.Recipe));
    }
}
