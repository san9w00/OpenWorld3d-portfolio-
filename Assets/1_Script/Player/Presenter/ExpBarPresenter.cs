using UnityEngine;

public class ExpBarPresenter : MonoBehaviour
{
    [SerializeField] private UIBarView view;

    private PlayerLevelSystem levelSystem;

    private void Start()
    {
        levelSystem = PlayerLevelSystem.Instance;

        if (levelSystem == null)
        {
            Debug.LogError("PlayerLevelSystem Instance ¾øÀ½!");
            return;
        }

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
