using UnityEngine;

public enum QuestType
{
    KillMonster,
    ReachLevel,
}

[CreateAssetMenu(menuName = "Quest/Quest Data")]
public class QuestDataSO : ScriptableObject
{
    [Header("Info")]
    public string questName;

    [TextArea]
    public string npcDialogue;

    [TextArea]
    public string progressDialogue = "Quest is Progressing now..";

    [TextArea]
    public string completeDialogue = "Good job. Here your Reward.";

    public QuestType questType;

    public int targetAmount;
    public int goldReward;
}
