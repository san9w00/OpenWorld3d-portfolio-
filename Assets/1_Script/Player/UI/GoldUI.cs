using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;

    private PlayerStatus playerStatus;

    private void Start()
    {
        playerStatus = PlayerStatus.Instance;

        if (playerStatus != null)
        {
            playerStatus.OnGoldChanged += UpdateGoldUI;
            UpdateGoldUI();
        }
    }

    private void OnDestroy()
    {
        if (playerStatus != null)
        {
            playerStatus.OnGoldChanged -= UpdateGoldUI;
        }
    }

    private void UpdateGoldUI()
    {
        goldText.text = playerStatus.Gold.ToString();
    }
}
