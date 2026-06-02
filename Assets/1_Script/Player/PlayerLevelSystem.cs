using System;
using UnityEngine;
using static StatData;

public class PlayerLevelSystem : MonoBehaviour
{
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

        SoundManager.Instance.PlaySFX(SFXType.LevelUP);

        // 업그레이드 포인트 변경 이벤트 발행
        EventBus.Publish(new UpgradePointChangedEvent(upgradePoint));

        // Exp UI 이벤트
        OnExpChanged?.Invoke(new StatData(currentExp, requiredExp));

        // Level UI 이벤트
        OnLevelUIChanged?.Invoke(new LevelUIData(currentLevel, currentExp, requiredExp));

        // 레벨업 이벤트 발행
        EventBus.Publish(new LevelUpEvent(currentLevel));

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

        // 포인트 변경 이벤트 발행
        EventBus.Publish(new UpgradePointChangedEvent(upgradePoint));

        return true;
    }
}
