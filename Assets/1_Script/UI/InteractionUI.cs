using TMPro;
using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    public static InteractionUI instance;

    [SerializeField] private GameObject interactionPanel;
    [SerializeField] private TextMeshProUGUI interactionText;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Hide();
    }

    public void Show(string text)
    {
        interactionPanel.SetActive(true);
        interactionText.text = $"[F] {text}";      
    }

    public void Hide()
    {
        interactionPanel.SetActive(false);
    }
}
