using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestListUI : MonoBehaviour
{
    public static QuestListUI Instance;

    [SerializeField] private GameObject panel;
    [SerializeField] private Transform content;
    [SerializeField] private QuestSlotUI slotPrefab;

    private List<QuestSlotUI> slots = new();

    [SerializeField]
    private TutorialSlotUI tutorialSlot;

    private void Awake()
    {
        Instance = this;

        panel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ToggleUpgradePanel();
        }
    }

    public void AddQuest(QuestRuntimeData quest)
    {
        QuestSlotUI slot = Instantiate(slotPrefab, content);
        slot.Bind(quest);

        slots.Add(slot);
    }

    public void Refresh()
    {
        for (int i = slots.Count - 1; i>= 0; i--)
        {
            if (!QuestManager.Instance.AcceptedQuests.Contains(slots[i].Quest))
            {
                Destroy(slots[i].gameObject);
                slots.RemoveAt(i);
                continue;
            }

            slots[i].Refresh();
        }
    }

    public void RefreshTutorial(TutorialRuntimeData tutorial)
    {
        tutorialSlot.Bind(tutorial);
    }

    void ToggleUpgradePanel()
    {
        bool isOpen = !panel.activeSelf;
        panel.SetActive(isOpen);
    }
}
