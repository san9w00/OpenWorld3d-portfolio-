using UnityEngine;

public class TeleportUI : MonoBehaviour
{
    public static TeleportUI Instance;

    [SerializeField] private GameObject panel;
    private CampFire target;

    private void Awake()
    {
        Instance = this;

        panel.SetActive(false);
    }

    public void Open(CampFire campFire)
    {
        target = campFire;
        panel.SetActive(true);
    }

    public void OnClickYes()
    {
        panel.SetActive(false);
        if (target == null) return;

        TeleportSystem.Instance.Teleport(target.transform.position);
    }

    public void OnClickNo()
    {
        panel.SetActive(false);
    }
}
