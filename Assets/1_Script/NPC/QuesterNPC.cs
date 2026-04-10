using UnityEngine;

public class QuesterNPC : MonoBehaviour, IInteractable
{
    [SerializeField] private QuestDataSO baseQuest;
    [SerializeField] private DialogueUI dialogueUI;

    private Animator animator;

    private int questTier = 1;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Interact()
    {
        QuestRuntimeData quest = QuestManager.Instance.GetQuest(this);
        animator.Play("Dialogue");

        // 아직 수락 안한 상태
        if (quest == null)
        {
            dialogueUI.ShowQuestOffer(
                baseQuest.npcDialogue,
                AcceptQuest,
                null);

            return;
        }

        // 완료함
        if (quest.isCompleted)
        {
            dialogueUI.ShowSimple(
                baseQuest.completeDialogue,
                () => QuestManager.Instance.CompleteQuest(quest, FindAnyObjectByType<PlayerStatus>()));

            return;
        }

        // 아직 진행중
        dialogueUI.ShowSimple(baseQuest.progressDialogue, null);
    }

    private void AcceptQuest()
    {
        int target = baseQuest.targetAmount * questTier;

        QuestManager.Instance.AcceptQuest(this, baseQuest, target);
    }

    public void IncreaseQuestTier()
    {
        questTier++;
    }

    public bool CanInteract() => true;

    public string GetInteractText() => "Talk";
}
