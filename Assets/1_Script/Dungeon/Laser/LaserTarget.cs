using UnityEngine;

public class LaserTarget : MonoBehaviour, ILasetInteractable
{
    [SerializeField] private DoorDG door;

    private bool activated;

    public void OnLaserHit()
    {
        if (activated) return;

        activated = true;

        door.OpenDoor(activated);
    }
}
