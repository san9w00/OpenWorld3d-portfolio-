using UnityEngine;

public class ItemNotifierSystem : MonoBehaviour
{
    [SerializeField] private Transform notifierRoot;
    [SerializeField] private ItemNotifierUI notifierPrefabUI;

    private void OnEnable()
    {
        EventBus.Subscribe<ItemAddedEvent>(OnItemAdded);
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<ItemAddedEvent>(OnItemAdded);
    }

    private void OnItemAdded(ItemAddedEvent evt)
    {
        ShowNotifier(evt.Item, evt.Amount);
    }

    private void ShowNotifier(ItemSO item, int amount)
    {
        ItemNotifierUI notifier = Instantiate(notifierPrefabUI, notifierRoot);

        notifier.Setup(item, amount);
    }
}
