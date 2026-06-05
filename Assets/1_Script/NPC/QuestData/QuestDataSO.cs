using UnityEngine;

public enum EnemyType
{
    Slime,
    Turtle,
    Bear,
    Skeleton,
    Boss,
    Yeti,
    Magma,
}

public enum QuestGoalType
{
    KillEnemy,
    ReachLevel,
}

[CreateAssetMenu(menuName = "Quest/Quest Data")]
public class QuestDataSO : ScriptableObject
{
    [Header("Info")]
    public int questID;
    public string questName;

    [TextArea]
    public string npcDialogue;

    [TextArea]
    public string progressDialogue = "Quest is Progressing now..";

    [TextArea]
    public string completeDialogue = "Good job. Here your Reward.";

    public QuestGoalType goalType;

    [Header("Kill Enemy")]
    public EnemyType targetEnemyType;

    [Header("Goal")]
    public int targetAmount;

    [Header("Reward")]
    public int goldReward;
}
