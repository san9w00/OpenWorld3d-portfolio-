using TMPro;
using UnityEngine;
using static StatData;

public class UILevelTextView : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI levelText;

    [SerializeField] private TextMeshProUGUI expText;

    // 레벨/경험치 표시
    public void SetText(LevelUIData data)
    {
        levelText.text = $"Level {data.Level}";

        expText.text =
            $"{data.CurrentExp} / {data.RequiredExp}";
    }
}
