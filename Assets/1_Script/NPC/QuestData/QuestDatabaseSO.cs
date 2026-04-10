using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Quest Database")]
public class QuestDatabaseSO : ScriptableObject
{
    public List<QuestDataSO> quests;

    private Dictionary<int, QuestDataSO> cache;

    public void Init()
    {
        cache = new Dictionary<int, QuestDataSO>();

        foreach (QuestDataSO quest in quests)
        {
            cache[quest.questID] = quest;
        }
    }

    public QuestDataSO GetQuest(int id)
    {
        if (cache == null)
            Init();

        return cache[id];
    }
}
