using UnityEngine;

public class DoorDG : MonoBehaviour
{
    [SerializeField] private float openHeight = 4f;
    [SerializeField] private float moveSpeed = 3f;

    private Vector3 closedPos;
    private Vector3 openPos;

    private bool isOpen;

    private void Start()
    {
        closedPos = transform.position;
        openPos = closedPos + Vector3.up * openHeight;
    }

    private void Update()
    {
        Vector3 target = isOpen ? openPos : closedPos;
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }

    public void OpenDoor(bool value)
    {
        isOpen = value;
    }
}
