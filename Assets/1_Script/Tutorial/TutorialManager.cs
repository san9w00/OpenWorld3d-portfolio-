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

    private void OnEnable()
    {
        EventBus.Subscribe<GamePlayEvent>(HandleGamePlayEvent);
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<GamePlayEvent>(HandleGamePlayEvent);
    }

    private void HandleGamePlayEvent(GamePlayEvent e)
    {
        if (currentTutorial == null)
            return;

        if (!currentTutorial.isStarted)
            return;

        if (currentTutorial.isCompleted)
            return;

        if (currentTutorial.stepData.goalType != e.eventType)
            return;

        currentTutorial.currentAmount += e.amount;

        if (currentTutorial.currentAmount >=
        currentTutorial.stepData.targetAmount)
        {
            currentTutorial.currentAmount =
                currentTutorial.stepData.targetAmount;

            CompleteTutorial();
        }

        QuestListUI.Instance.RefreshTutorial(currentTutorial);
        Debug.Log(e.eventType);
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
            isStarted = false,
            isCompleted = false
        };

        ShowTutorialDialogue();
    }

    private void ShowTutorialDialogue()
    {
        TutorialStepSO step = currentTutorial.stepData;

        DialogueUI.Instance.SetPortrait(step.portraitPrefab);

        DialogueManager.Instance.Show(new DialogueRequest
        {
            text = step.dialogue,

            onContinue = () =>
            {
                BeginTutorialMission();
            }
        });     
    }

    private void BeginTutorialMission()
    {
        currentTutorial.isStarted = true;
        QuestListUI.Instance.RefreshTutorial(currentTutorial);
    }

    public void ReportProgress(GamePlayEventType goalType, int amount = 1)
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


