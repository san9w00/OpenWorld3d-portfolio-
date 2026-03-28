using UnityEngine;

public class TeleportUI : MonoBehaviour
{
    public static TeleportUI Instance;

    private CampFire target;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void Open(CampFire campFire)
    {
        target = campFire;
        gameObject.SetActive(true);
    }

    public void OnClickYes()
    {
        gameObject.SetActive(false);

        if (target == null) return;

        TeleportSystem.Instance.Teleport(target.transform.position);
    }

    public void OnClickNo()
    {
        gameObject.SetActive(false);
    }
}
