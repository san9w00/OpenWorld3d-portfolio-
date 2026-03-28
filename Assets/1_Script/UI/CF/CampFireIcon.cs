using UnityEngine;
using UnityEngine.UI;

public class CampFireIcon : MonoBehaviour
{
    [SerializeField] private string id;

    private CampFire campFire;
    private Button button;

    public string ID => id;

    public void Init()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(OnClick);

        button.interactable = false;
        gameObject.SetActive(false);
    }

    public void Activate(CampFire campFire)
    {
        this.campFire = campFire;

        gameObject.SetActive(true);
        button.interactable = true;
    }

    void OnClick()
    {
        TeleportUI.Instance.Open(campFire);
    }
}
