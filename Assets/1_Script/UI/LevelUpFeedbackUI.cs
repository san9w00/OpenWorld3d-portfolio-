using System.Collections;
using UnityEngine;

public class LevelUpFeedbackUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private CanvasGroup exclamationUI; // ´À³¦Ç¥ UI

    [Header("Settings")]
    [SerializeField] private float fadeSpeed = 2f;

    private void Start()
    {
        exclamationUI.alpha = 0f;
    }

    private void Update()
    {
        HandleExclamation();
    }

    private void HandleExclamation()
    {
        if(PlayerLevelSystem.Instance.UpgradePoint > 0)
        {
            float alpha = Mathf.PingPong(Time.time * fadeSpeed, 1f);
            exclamationUI.alpha = alpha;
        }
        else
        {
            exclamationUI.alpha = 0f;
        }
    }
}
