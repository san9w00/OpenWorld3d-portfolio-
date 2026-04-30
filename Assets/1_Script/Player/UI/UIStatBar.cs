using UnityEngine;
using UnityEngine.UI;

public class UIStatBar : MonoBehaviour
{
    [SerializeField] private StatType targetType;
    [SerializeField] private Image fillImage;

    private PlayerStatus playerStatus;
    private PlayerLevelSystem levelSystem;

    private void Awake()
    {
        playerStatus = FindAnyObjectByType<PlayerStatus>();
        levelSystem = FindAnyObjectByType<PlayerLevelSystem>();
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

    private void Start()
    {
        ForceInit(); // 최초 1회 안전 초기화
    }

    private void ForceInit()
    {
        switch (targetType)
        {
            case StatType.HP:
                if (playerStatus != null)
                    UpdateFill(playerStatus.curHP, playerStatus.MaxHP);
                break;

            case StatType.Stamina:
                if (playerStatus != null)
                    UpdateFill(playerStatus.curStamina, playerStatus.MaxStamina);
                break;

            case StatType.Exp:
                if (levelSystem != null)
                    UpdateFill(levelSystem.CurrentExp, levelSystem.RequiredExp);
                break;
        }
    }

    private void UpdateUI(StatType type, float current, float max)
    {
        if (type != targetType) return;

        UpdateFill(current, max);
    }    

    private void UpdateFill(float current, float max)
    {
        if (fillImage == null) return;

        float ratio = (max <= 0) ? 0f : current / max;
        fillImage.fillAmount = Mathf.Clamp01(ratio);
    }
}
