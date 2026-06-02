using UnityEngine;
using static StatData;

public class LevelTextPresenter : MonoBehaviour
{
    [SerializeField] private UILevelTextView view;

    [SerializeField] private PlayerLevelSystem levelSystem;

    private void Start()
    {
        // 이벤트 연결
        levelSystem.OnLevelUIChanged += UpdateView;

        // 최초 동기화
        UpdateView(new LevelUIData(
            levelSystem.CurrentLevel,
            levelSystem.CurrentExp,
            levelSystem.RequiredExp));
    }

    private void OnDestroy()
    {
        if (levelSystem != null)
        {
            levelSystem.OnLevelUIChanged -= UpdateView;
        }
    }

    private void UpdateView(LevelUIData data)
    {
        view.SetText(data);
    }
}
