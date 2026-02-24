using UnityEngine;
using UnityEngine.UI;

public class UIStatBar : MonoBehaviour
{
    [SerializeField] private StatType targetType;
    [SerializeField] private Image fillImage;

    [Header("Reference")]
    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private PlayerLevelSystem levelSystem;

    private void Start()
    {
        // 초기값 강제 세팅
        if (targetType == StatType.HP && playerStatus != null)
        {
            fillImage.fillAmount = playerStatus.curHP / playerStatus.MaxHP;
        }
        else if (targetType == StatType.Stamina && playerStatus != null)
        {
            fillImage.fillAmount = playerStatus.curStamina / playerStatus.MaxStamina;
        }
        else if (targetType == StatType.Exp && levelSystem != null)
        {
            fillImage.fillAmount = (float)levelSystem.CurrentExp / levelSystem.RequiredExp;
        }
    }

    private void OnEnable()
    {
        // 이벤트 구독
        if (playerStatus != null)
        {
            playerStatus.OnStatChanged += UpdateUI;
        }

        if (levelSystem != null)
        {
            levelSystem.OnLevelStatChanged += UpdateUI;
        }
    }

    private void OnDisable()
    {
        // 이벤트 구독 해제
        if (playerStatus != null)
            playerStatus.OnStatChanged -= UpdateUI;

        if (levelSystem != null)
            levelSystem.OnLevelStatChanged -= UpdateUI;
    }

    private void UpdateUI(StatType type, float current, float max)
    {
        if (type == targetType)
        {
            fillImage.fillAmount = current / max;
        }
    }    
}
