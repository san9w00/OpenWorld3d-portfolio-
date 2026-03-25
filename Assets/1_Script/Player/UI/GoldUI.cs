using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private PlayerStatus playerStatus;

    private void Start()
    {
        UpdateGoldUI();
        playerStatus.OnGoldChanged += UpdateGoldUI;
    }

    private void OnDestroy()
    {
        playerStatus.OnGoldChanged -= UpdateGoldUI;
    }

    private void UpdateGoldUI()
    {
        goldText.text = playerStatus.Gold.ToString();
    }
}
