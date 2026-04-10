using TMPro;
using UnityEngine;

public class QuestSlotUI : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text progressText;

    public QuestRuntimeData Quest { get; private set; }

    public void Bind(QuestRuntimeData quest)
    {
        Quest = quest;
        Refresh();
    }

    public void Refresh()
    {
        if (Quest == null)
            return;

        titleText.text = Quest.questData.questName;

        progressText.text = $"{Quest.currentAmount} / {Quest.targetAmount}";

        if (Quest.isCompleted )
        {
            progressText.text += "  (Complete!)";
        }
    }
}
