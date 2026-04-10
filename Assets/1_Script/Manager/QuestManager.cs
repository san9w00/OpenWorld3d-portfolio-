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
    }
}
