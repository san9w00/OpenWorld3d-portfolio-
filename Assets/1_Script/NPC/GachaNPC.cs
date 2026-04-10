using UnityEngine;

public class GachaNPC : MonoBehaviour, IInteractable
{
    [SerializeField] private GachaUI gachaUI;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        animator.Play("Dialogue");

        Cursor.lockState = CursorLockMode.None;
        gachaUI.Open();
    }

    public bool CanInteract() => true;

    public string GetInteractText()
    {
        return "Open/Close Gacha";
    }
}
