using UnityEngine;

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
    public GamePlayEventType goalType;

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
