using UnityEngine;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    [SerializeField] private List<TutorialStepSO> tutorialSteps;

    private int currentIndex;

    private TutorialRuntimeData currentTutorial;

    public TutorialRuntimeData CurrentTutorial => currentTutorial;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartTutorial();
    }

    private void StartTutorial()
    {
        if (currentIndex >= tutorialSteps.Count)
            return;

        currentTutorial = new TutorialRuntimeData
        {
            stepData = tutorialSteps[currentIndex],
            currentAmount = 0,
            isCompleted = false
        };

        ShowTutorialDialogue();

        QuestListUI.Instance.RefreshTutorial(currentTutorial);
    }

    private void ShowTutorialDialogue()
    {
        TutorialStepSO step = currentTutorial.stepData;

        DialogueManager.Instance.Show(new DialogueRequest
        {
            text = step.dialogue
        });

        DialogueUI.Instance.SetPortrait(step.goddessPortrait);
    }

    public void ReportProgress(TutorialGoalType goalType, int amount = 1)
    {
        if (currentTutorial == null)
            return;

        if (currentTutorial.isCompleted)
            return;

        if (currentTutorial.stepData.goalType != goalType)
            return;

        currentTutorial.currentAmount += amount;

        if (currentTutorial.currentAmount >= currentTutorial.stepData.targetAmount)
        {
            CompleteTutorial();
        }

        QuestListUI.Instance.RefreshTutorial(currentTutorial);
    }

    private void CompleteTutorial()
    {
        currentTutorial.isCompleted = true;

        currentIndex++;

        StartTutorial();
    }
}


