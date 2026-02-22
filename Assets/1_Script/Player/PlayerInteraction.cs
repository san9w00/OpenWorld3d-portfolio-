using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Overlap Settings")]
    [SerializeField] private float interactionRadius = 3f;
    [SerializeField] private LayerMask interactableLayer;

    private IInteractable currentTarget;

    private void Update()
    {
        DetectTarget();
        HandleInput();
    }

    private void DetectTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactionRadius, interactableLayer);

        float closestDistance = float.MaxValue;
        IInteractable closest = null;

        foreach (var hit in hits)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();
            if (interactable == null) continue;
            if (!interactable.CanInteract()) continue; // 연결할수 있는가!! bool

            float distance = Vector3.Distance(transform.position, hit.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = interactable;
            }

            currentTarget = closest;

            // ui 안내 표시
        }
    }

    private void HandleInput()
    {
        if (currentTarget == null) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            currentTarget.Interact();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
