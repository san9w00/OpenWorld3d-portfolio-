using UnityEngine;

public enum QuestType
{
    KillMonster,
    ReachLevel,
    CollectItem
}

[CreateAssetMenu(menuName = "Quest/Quest Data")]
public class QuestDataSO : ScriptableObject
{
    [Header("Info")]
    public string questName;
    [TextArea] public string description;

    [Header("Quest Type")]
    public QuestType questType;

    [Header("Goal")]
    public int targetAmount;

    [Header("Reward")]
    public int goldReward;
}
