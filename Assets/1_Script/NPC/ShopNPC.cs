using UnityEngine;

public class ShopNPC : MonoBehaviour, IInteractable
{
    [SerializeField] private ShopData shopData;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Interact()
    {
        animator.Play("Dialogue");

        ShopUI.Instance.Open(shopData);
        Cursor.lockState = CursorLockMode.None;
    }

    public bool CanInteract() => true;

    public string GetInteractText()
    {
        return "Open/Close Shop";
    }
}
