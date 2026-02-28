using UnityEngine;

public class ShopNPC : MonoBehaviour, IInteractable
{
    [SerializeField] private ShopData shopData;

    public void Interact()
    {
        ShopUI.Instance.Open(shopData);
        Cursor.lockState = CursorLockMode.None;
    }

    public bool CanInteract() => true;

    public string GetPrompt()
    {
        return "F - Open Shop";
    }
}
