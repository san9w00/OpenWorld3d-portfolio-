using System.Collections.Generic;
using UnityEngine;

public class QuestListUI : MonoBehaviour
{
    public static QuestListUI Instance;

    [SerializeField] private Transform content;
    [SerializeField] private QuestSlotUI slotPrefab;

    private List<QuestSlotUI> slots = new();

    private void Awake()
    {
        Instance = this;
    }
}
