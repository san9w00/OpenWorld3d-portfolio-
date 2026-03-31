using UnityEngine;

public class QuesterNPC : MonoBehaviour, IInteractable
{
    [Header("Quest")]
    [SerializeField] private QuestDataSO baseQuest;

    [Header("UI")]
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private QuestUI questUI;

    private int currentQuestTier = 1;
    private bool hasAcceptedQuest;

    public void Interact()
    {
        Cursor.lockState = CursorLockMode.None;

        if (!hasAcceptedQuest)
        {
            ShowDialogue();
            return;
        }

        OpenQuestUI();
    }

    private void ShowDialogue()
    {
        dialogueUI.Show(
            "몬스터를 처치해줄 수 있나?",
            OnDialogueFinished);
    }

    private void OnDialogueFinished()
    {
        hasAcceptedQuest = true;

        QuestManager.Instance.StartQuest(this, GetCurrentQuestTarget(), GetCurrentReward());

        OpenQuestUI();
    }

    public int GetCurrentQuestTarget()
    {
        return baseQuest.targetAmount * currentQuestTier;
    }

    public int GetCurrentReward()
    {
        return baseQuest.goldReward * currentQuestTier;
    }

    public void CompleteQuest()
    {
        currentQuestTier++;
        hasAcceptedQuest = false;
    }

    private void OpenQuestUI()
    {
        questUI.Open(this);
    }

    public bool CanInteract() => true;

    public string GetInteractText()
    {
        return "Talk";
    }    
}
