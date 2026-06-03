using UnityEngine;

public class KeyDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private DoorDG door;
    [SerializeField] private ResourceItemSO requiredKey;

    [Header("Text")]
    [SerializeField] private string openText = "Use Key";
    [SerializeField] private string needKeyText = "Need Key";

    private bool isOpened;

    public void Interact()
    {
        if (isOpened)
            return;

        if (!HasKey())
            return;

        PlayerInventory.Instance.RemoveItem(requiredKey, 1);

        door.OpenDoor(true);

        isOpened = true;
    }

    public bool CanInteract()
    {
        return !isOpened;
    }

    public string GetInteractText()
    {
        if (isOpened)
            return "";

        return HasKey() ? openText : needKeyText;
    }

    private bool HasKey()
    {
        return PlayerInventory.Instance.GetItemCount(requiredKey) > 0;
    }
}
