using UnityEngine;

public class OpenDoorfloor : MonoBehaviour
{
    [SerializeField] private BoxCollider doorCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Pushable"))
        {
            doorCollider.enabled = false;
            Debug.Log("발판 작동! 문열림!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        doorCollider.enabled = true;
    }
}
