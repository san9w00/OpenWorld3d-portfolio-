using UnityEngine;

public class LevelNotifyUI : MonoBehaviour
{
    [SerializeField] private PlayerLevelSystem levelSystem;

    private void OnEnable()
    {
        levelSystem.OnLevelStatChanged += OnNotify;
    }

    private void OnDisable()
    {
        levelSystem.OnLevelStatChanged -= OnNotify;
    }

    private void OnNotify(StatType type, float current, float max)
    {

    }
}
