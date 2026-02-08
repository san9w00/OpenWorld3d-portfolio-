using UnityEngine;
using UnityEngine.UI;

public class UIStatBar : MonoBehaviour
{
    [SerializeField] private StatType targetType;
    [SerializeField] private Image fillImage;
    [SerializeField] private PlayerStatus playerStatus;

    private void OnEnable()
    {
        // 이벤트 구독
        if (playerStatus != null)
        {
            playerStatus.OnStatChanged += UpdateUI;
        }
    }

    private void OnDisable()
    {
        // 이벤트 구독 해제
        if (playerStatus != null)
            playerStatus.OnStatChanged -= UpdateUI;
    }

    private void UpdateUI(StatType type, float current, float max)
    {
        if (type == targetType)
        {
            fillImage.fillAmount = current / max;
        }
    }    
}
