using TMPro;
using UnityEngine;

public class UILevelText : MonoBehaviour
{
    [SerializeField] private PlayerLevelSystem levelSystem;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI expText;

    private void OnEnable()
    {
        levelSystem.OnLevelStatChanged += UpdateText;
    }
    private void OnDisable()
    {
        levelSystem.OnLevelStatChanged -= UpdateText;
    }

    private void Start()
    {
        Refresh();
    }

    private void UpdateText(StatType type, float current, float max)
    {
        Refresh();
    }

    private void Refresh()
    {
        levelText.text = $"Level {levelSystem.CurrentLevel}";
        expText.text = $"{levelSystem.CurrentExp} / {levelSystem.RequiredExp}";
    }
}
