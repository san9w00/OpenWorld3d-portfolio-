using System;
using UnityEngine;
using static StatData;

public class PlayerLevelSystem : MonoBehaviour
{
    public static PlayerLevelSystem Instance;

    private int currentLevel = 1;
    private int currentExp = 0;
    private int requiredExp = 20;
    private int upgradePoint = 0;

    public int CurrentLevel => currentLevel;
    public int CurrentExp => currentExp;
    public int RequiredExp => requiredExp;
    public int UpgradePoint => upgradePoint;

    // EXP UI 이벤트
    public event Action<StatData> OnExpChanged;

    // 레벨 텍스트 UI 이벤트
    public event Action<LevelUIData> OnLevelUIChanged;

    // 레벨 변경 이벤트
    public event Action<int> OnLevelChanged;

    // 업그레이드 포인트 변경 이벤트
    public event Action<int> OnUpgradePointChanged;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        OnExpChanged?.Invoke(new StatData(currentExp, requiredExp));
        OnLevelUIChanged?.Invoke(new LevelUIData(currentLevel, currentExp, requiredExp));
    }

    public void AddExp(int amount)
    {
        currentExp += amount;

        while (currentExp >= requiredExp)
        {
            currentExp -= requiredExp;
            LevelUp();
        }

        // UI 업데이트
        OnExpChanged?.Invoke(new StatData(currentExp, requiredExp));
        OnLevelUIChanged?.Invoke(new LevelUIData(currentLevel, currentExp, requiredExp));
    }

    private void LevelUp()
    {
        currentLevel++;
        upgradePoint++;
        requiredExp *= 2;

        // 레벨업 시 체력 전체 회복
        PlayerStatus.Instance.FullHeal();

        // 레벨 이벤트
        OnLevelChanged?.Invoke(currentLevel);

        // 업그레이드 포인트 이벤트
        OnUpgradePointChanged?.Invoke(upgradePoint);

        // Exp UI 이벤트
        OnExpChanged?.Invoke(
            new StatData(currentExp, requiredExp));

        // Level UI 이벤트
        OnLevelUIChanged?.Invoke(new LevelUIData(currentLevel, currentExp, requiredExp));

        EventBus.Publish(new VFXEvent(transform.position,
            VFXActionType.LevelUp,
            VFXSwordType.None
        ));

        Debug.Log($"레벨업! 현재 레벨: {currentLevel}");
    }

    public bool UseUpgradePoint()
    {
        if (upgradePoint <= 0)
            return false;

        upgradePoint--;

        // 포인트 변경 이벤트
        OnUpgradePointChanged?.Invoke(upgradePoint);
        return true;
    }
}
