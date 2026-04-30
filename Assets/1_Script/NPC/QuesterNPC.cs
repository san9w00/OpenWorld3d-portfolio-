using System.Collections.Generic;
using UnityEngine;

public class QuesterNPC : MonoBehaviour, IInteractable
{
    [SerializeField] private QuestDataSO baseQuest;
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
        QuestRuntimeData quest = QuestManager.Instance.GetQuest(this);
        animator.Play("Dialogue");

        // 더이상 줄 퀘스트 없음
        if (CurrentQuestData == null)
        {
            DialogueUI.Instance.ShowSimple("No more Quest for You! Thank you for your help!", null);
            return;
        }

        // 아직 수락 안한 상태
        if (quest == null)
        {
            DialogueUI.Instance.ShowQuestOffer(
                CurrentQuestData.npcDialogue,
                AcceptQuest,
                null);

            return;
        }

        // 완료함
        if (quest.isCompleted)
        {
            DialogueUI.Instance.ShowSimple(
                baseQuest.completeDialogue,
                () => QuestManager.Instance.CompleteQuest(quest, FindAnyObjectByType<PlayerStatus>()));

            return;
        }

        // 아직 진행중
        DialogueUI.Instance.ShowSimple(baseQuest.progressDialogue, null);
    }

    private void AcceptQuest()
    {
        var questData = CurrentQuestData;

        QuestManager.Instance.AcceptQuest(this, questData, questData.targetAmount);
    }

    public void AdvanceQuest()
    {
        currentIndex++;
    }

    public bool CanInteract() => true;

    public string GetInteractText() => "Talk";
}
