using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    private QuesterNPC currentQuestNPC;

    public int CurrentKillCount { get; private set; }
    public int RequiredKillCount { get; private set; }
    public int RewardGold { get; private set; }

    public bool HasQuest => currentQuestNPC != null;

    private void Awake()
    {
        Instance = this;
    }

    public void StartQuest(QuesterNPC npc, int requiredKills, int rewardGold)
    {
        currentQuestNPC = npc;

        RequiredKillCount = requiredKills;
        RewardGold = rewardGold;
        CurrentKillCount = 0;

        QuestUI.Instance.Refresh();
    }

    public void AddKill()
    {
        if (!HasQuest)
            return;

        CurrentKillCount++;

        QuestUI.Instance.Refresh();

        Debug.Log($"Äù½ºÆ® ÁøÇà: {CurrentKillCount}/{RequiredKillCount}");
    }

    public bool IsQuestComplete()
    {
        return CurrentKillCount >= RequiredKillCount;
    }

    public void ClaimReward(PlayerStatus playerStatus)
    {
        if (!IsQuestComplete()) return;

        playerStatus.AddGold(RewardGold);

        currentQuestNPC.CompleteQuest();

        currentQuestNPC = null;

        CurrentKillCount = 0;
        RequiredKillCount = 0;
        RewardGold = 0;

        QuestUI.Instance.Close();
    }
}
