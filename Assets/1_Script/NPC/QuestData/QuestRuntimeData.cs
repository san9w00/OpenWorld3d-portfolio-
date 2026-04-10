using TMPro.EditorUtilities;
using UnityEngine;

[System.Serializable]
public class QuestRuntimeData
{
    public QuestDataSO questData;

    public int currentAmount;
    public int targetAmount;

    public bool isCompleted;

    public QuesterNPC ownerNPC;
}
