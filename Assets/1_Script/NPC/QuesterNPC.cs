using UnityEngine;

public class QuesterNPC : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public bool CanInteract() => true;

    public string GetInteractText()
    {
        return "Talk";
    }    
}
