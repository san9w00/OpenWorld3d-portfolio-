using UnityEngine;

public class ExpBarPresenter : MonoBehaviour
{
    [SerializeField] private UIBarView view;

    [SerializeField] private PlayerLevelSystem levelSystem;

    private void Start()
    {
        levelSystem.OnExpChanged += UpdateView;
    }

    private void OnDestroy()
    {
        if (levelSystem != null)
        {
            levelSystem.OnExpChanged -= UpdateView;
        }
    }

    private void UpdateView(StatData data)
    {
        view.SetFill(data.Normalized);
    }
}
