using UnityEngine;

public class DoorPressure : MonoBehaviour
{
    [SerializeField] private DoorDG door;

    private int countObj = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("PuzzleOBJ")) return;

        countObj++;

        if(countObj == 1)
        {
            door.OpenDoor(true);
            Debug.Log("πÆ¿Ã ø≠∏∞¥Ÿ!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("PuzzleOBJ")) return;

        countObj--;

        if(countObj == 0)
        {
            door.OpenDoor(false);
            Debug.Log("πÆ¿Ã ¥›»˘¥Ÿ!");
        }
    }
}
