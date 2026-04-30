using System;
using UnityEngine;

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

    public event Action<StatType, float, float> OnLevelStatChanged;

    private void Start()
    {
        OnLevelStatChanged?.Invoke(StatType.Level, currentLevel, currentLevel);
        OnLevelStatChanged?.Invoke(StatType.Exp, currentExp, requiredExp);
    }

    public void AddExp(int amount)
    {
        currentExp += amount;

        while (currentExp >= requiredExp)
        {
            currentExp -= requiredExp;
            LevelUp();
        }

        OnLevelStatChanged?.Invoke(StatType.Exp, currentExp, requiredExp);
    }

    private void LevelUp()
    {
        currentLevel++;
        upgradePoint++;
        requiredExp *= 2;

        OnLevelStatChanged?.Invoke(StatType.Level, currentLevel, currentLevel);

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
        return true;
    }


    // Load
    public void LoadLevelData(int level, int exp, int upgrade) // 데이터 강제 복원
    {
        currentLevel = level;
        currentExp = exp;
        upgradePoint = upgrade;

        OnLevelStatChanged?.Invoke(StatType.Level, currentLevel, currentLevel);
        OnLevelStatChanged?.Invoke(StatType.Exp, currentExp, requiredExp);
    }
}
