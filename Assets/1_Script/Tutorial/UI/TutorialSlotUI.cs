using UnityEngine;
using TMPro;

public class TutorialSlotUI : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text progressText;

    public void Bind(TutorialRuntimeData tutorial)
    {
        titleText.text = tutorial.stepData.objectiveText;

        progressText.text =
            $"{tutorial.currentAmount} / " +
            $"{tutorial.stepData.targetAmount}";
    }
}
