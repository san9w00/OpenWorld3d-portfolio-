using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    private List<QuestRuntimeData> acceptedQuests = new();

    public IReadOnlyList<QuestRuntimeData> AcceptedQuests => acceptedQuests;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        EventBus.Subscribe<EnemyKilledEvent>(HandleEnemyKilled);
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<EnemyKilledEvent>(HandleEnemyKilled);
    }

    private void HandleEnemyKilled(EnemyKilledEvent e)
    {
        OnEnemyKilled(e.enemyType);
    }

    public bool HasQuest(QuesterNPC npc)
    {
        return acceptedQuests.Exists(q => q.ownerNPC == npc);
    }

    public QuestRuntimeData GetQuest(QuesterNPC npc)
    {
        return acceptedQuests.Find(q => q.ownerNPC == npc);
    }

    public void AcceptQuest(QuesterNPC npc, QuestDataSO data, int targetAmount)
    {
        if (HasQuest(npc))
            return;

        QuestRuntimeData runtime = new QuestRuntimeData
        {
            questData = data,
            currentAmount = 0,
            targetAmount = targetAmount,
            ownerNPC = npc,
            isCompleted = false
        };

        acceptedQuests.Add(runtime);

        QuestListUI.Instance.AddQuest(runtime);

        SoundManager.Instance.PlaySFX(SFXType.QuestUnLcok);
    }

    public void OnEnemyKilled(EnemyType deadEnemyType)
    {
        foreach (QuestRuntimeData quest in acceptedQuests)
        {
            if (quest.isCompleted)
                continue;

            if (quest.questData.goalType != QuestGoalType.KillEnemy)
                continue;

            if (quest.questData.targetEnemyType != deadEnemyType)
                continue;

            quest.currentAmount++;

            if (quest.currentAmount >= quest.targetAmount)
            {
                quest.currentAmount = quest.targetAmount;
                quest.isCompleted = true;
            }
        }

        QuestListUI.Instance.Refresh();
    }    

    public void CheckLevelQuest(int currentLevel)
    {
        foreach (QuestRuntimeData quest in acceptedQuests)
        {
            if (quest.questData.goalType != QuestGoalType.ReachLevel)
                continue;

            if (quest.isCompleted)
                continue;

            quest.currentAmount = currentLevel;

            if (currentLevel >= quest.targetAmount)
                quest.isCompleted = true;
        }

        QuestListUI.Instance.Refresh();
    }

    public void CompleteQuest(QuestRuntimeData quest, PlayerStatus playerStatus)
    {
        playerStatus.AddGold(quest.questData.goldReward);

        acceptedQuests.Remove(quest);

        quest.ownerNPC.AdvanceQuest(); // 다음퀘스트 넘어가기

        QuestListUI.Instance.Refresh();
    }
}
