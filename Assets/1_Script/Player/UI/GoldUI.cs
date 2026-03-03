using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private PlayerGold playerGold;

    private void Start()
    {
        UpdateGoldUI();
        playerGold.OnGoldChanged += UpdateGoldUI;
    }

    private void OnDestroy()
    {
        playerGold.OnGoldChanged -= UpdateGoldUI;
    }

    private void UpdateGoldUI()
    {
        goldText.text = playerGold.Gold.ToString();
    }
}
