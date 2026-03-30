using UnityEngine;

public class GachaNPC : MonoBehaviour, IInteractable
{
    [SerializeField] private GachaUI gachaUI;

    public void Interact()
    {
        Cursor.lockState = CursorLockMode.None;
        gachaUI.Open();
    }

    public bool CanInteract() => true;

    public string GetInteractText()
    {
        return "Open/Close Gacha";
    }
}
