using System.Collections.Generic;
using UnityEngine;

public class QuestListUI : MonoBehaviour
{
    public static QuestListUI Instance;

    [SerializeField] private GameObject panel;
    [SerializeField] private Transform content;
    [SerializeField] private QuestSlotUI slotPrefab;

    private List<QuestSlotUI> slots = new();

    private InputHandler _inputHandler;

    private void Awake()
    {
        Instance = this;

        _inputHandler = FindAnyObjectByType<InputHandler>();
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
            if (slots[i].Quest == null)
            {
                Destroy(slots[i].gameObject);
                slots.RemoveAt(i);
                continue;
            }

            slots[i].Refresh();
        }
    }

    void ToggleUpgradePanel()
    {
        bool isOpen = !panel.activeSelf;
        panel.SetActive(isOpen);
        _inputHandler.SetInventoryState(isOpen);
    }

    public void Close()
    {
        panel.SetActive(false);
        _inputHandler.SetInventoryState(false);
    }
}
