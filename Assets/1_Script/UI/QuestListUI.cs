using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
