using UnityEngine;

public class ShopNPC : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject shopUI;

    public void Interact()
    {
        shopUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public bool CanInteract()
    {
        return true;
    }

    public string GetPrompt()
    {
        return "F - Open Shop";
    }
}
