using System.Collections.Generic;
using UnityEngine;

public class QuesterNPC : MonoBehaviour, IInteractable
{
    [SerializeField] private List<int> questIDList;
    [SerializeField] private QuestDatabaseSO questDatabase;

    private int currentIndex = 0;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private QuestDataSO CurrentQuestData
    {
        get
        {
            if (currentIndex >= questIDList.Count)
                return null;

            return questDatabase.GetQuest(questIDList[currentIndex]);
        }
    }

    public void Interact()
    {
        animator.Play("Dialogue");

        QuestRuntimeData quest = QuestManager.Instance.GetQuest(this);

        // 더이상 퀘스트 없음
        if (CurrentQuestData == null)
        {
            ShowNoQuestDialogue();
            return;
        }

        // 퀘스트 아직 안받음
        if (quest == null)
        {
            ShowQuestOfferDialogue();
            return;
        }

        // 완료 상태
        if (quest.isCompleted)
        {
            ShowCompleteDialogue(quest);
            return;
        }

        // 진행중
        ShowProgressDialogue();
    }

    private void ShowNoQuestDialogue()
    {
        DialogueManager.Instance.Show(new DialogueRequest
        {
            text = "No more quests for you."
        });
    }

    private void ShowQuestOfferDialogue()
    {
        DialogueManager.Instance.Show(new DialogueRequest
        {
            text = CurrentQuestData.npcDialogue,

            choices = new List<DialogueChoice>
            {
                new DialogueChoice
                {
                    buttonText = "Accept",
                    action = AcceptQuest
                },

                new DialogueChoice
                {
                    buttonText = "Decline"
                }
            }
        });
    }

    private void ShowCompleteDialogue(QuestRuntimeData quest)
    {
        DialogueManager.Instance.Show(new DialogueRequest
        {
            text = quest.questData.completeDialogue,

            onContinue = () =>
            {
                PlayerStatus player =
                    FindAnyObjectByType<PlayerStatus>();

                QuestManager.Instance.CompleteQuest(
                    quest,
                    player);
            }
        });
    }

    private void ShowProgressDialogue()
    {
        DialogueManager.Instance.Show(new DialogueRequest
        {
            text = CurrentQuestData.progressDialogue
        });
    }

    private void AcceptQuest()
    {
        QuestDataSO questData = CurrentQuestData;

        QuestManager.Instance.AcceptQuest(this, questData, questData.targetAmount);
    }

    public void AdvanceQuest()
    {
        currentIndex++;
    }

    public bool CanInteract() => true;

    public string GetInteractText() => "Talk";
}
