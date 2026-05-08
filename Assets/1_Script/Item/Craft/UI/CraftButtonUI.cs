using UnityEngine;
using UnityEngine.UI;

public class CraftButtonUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private GameObject darkOverlay;
    [SerializeField] private Button button;

    private CraftRecipeViewModel viewModel;
    
    public void Setup(CraftRecipeViewModel vm)
    {
        viewModel = vm;

        icon.sprite = vm.Icon;

        Refresh();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);
    }

    public void Refresh()
    {
        darkOverlay.SetActive(!viewModel.CanCraft);
    }

    private void OnClick()
    {
        EventBus.Publish(new CraftRecipeSelectedEvent(viewModel));
    }
}
