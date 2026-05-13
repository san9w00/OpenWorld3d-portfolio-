using UnityEngine;

public enum TutorialGoalType
{
    Move,
    CollectWood,
    OpenCraft,
    CraftAxe,
    EquipWeapon,
    KillEnemy
}

[CreateAssetMenu(menuName = "Tutorial/Tutorial Step")]
public class TutorialStepSO : ScriptableObject
{
    [Header("Info")]
    public string title;

    [TextArea]
    public string dialogue;

    [TextArea]
    public string objectiveText;

    [Header("Goal")]
    public TutorialGoalType goalType;

    public int targetAmount = 1;

    [Header("Visual")]
    public Sprite goddessPortrait;
}

[System.Serializable]
public class TutorialRuntimeData
{
    public TutorialStepSO stepData;

    public int currentAmount;

    public bool isCompleted;
}
