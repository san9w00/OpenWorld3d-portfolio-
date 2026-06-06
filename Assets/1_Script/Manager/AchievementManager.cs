using System;
using System.Collections.Generic;
using UnityEngine;

public enum AchievementType
{
    FirstKill,
    FirstCampfire,
    FirstTeleport,
    FirstRest,
    FirstCraft,
    RichMan,
    Explorer,
}

[Serializable]
public class AchievementData
{
    public AchievementType Type;

    public string Title;

    public string Description;

    public bool IsUnlocked;
}

public class AchievementManager : MonoBehaviour
{
    private HashSet<AchievementType> unlocked = new();

    private void OnEnable()
    {
        EventBus.Subscribe<EnemyKilledEvent>(OnEnemyKilled);

        EventBus.Subscribe<CampFireActivatedEvent>(OnCampFireActivated);
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<EnemyKilledEvent>(OnEnemyKilled);
        EventBus.UnSubscribe<CampFireActivatedEvent>(OnCampFireActivated);
    }


    private void OnEnemyKilled(EnemyKilledEvent evt)
    {
        Unlock(AchievementType.FirstKill, "인생 첫 사냥");
    }

    private void OnCampFireActivated(CampFireActivatedEvent evt)
    {
        Unlock(AchievementType.FirstCampfire,"첫 휴식처 발견");
    }

    private void Unlock(AchievementType type, string title)
    {
        if (unlocked.Contains(type))
            return;

        unlocked.Add(type);

        EventBus.Publish(new AchievementUnlockedEvent(type, title));

        EventBus.Publish(new HintEvent($"신규 업적 : {title}",HintType.Achievement));
    }

}
