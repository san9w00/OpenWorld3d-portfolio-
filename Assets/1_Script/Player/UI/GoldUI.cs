using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;

    private void Start()
    {
        PlayerStatus.Instance.OnGoldChanged += UpdateGoldUI;
        UpdateGoldUI();
    }

    private void OnDestroy()
    {
        PlayerStatus.Instance.OnGoldChanged -= UpdateGoldUI;
    }

    private void UpdateGoldUI()
    {
        goldText.text = PlayerStatus.Instance.Gold.ToString();
    }
}
