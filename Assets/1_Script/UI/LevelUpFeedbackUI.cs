using System.Collections;
using UnityEngine;

public class LevelUpFeedbackUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private CanvasGroup exclamationUI; // ´À³¦Ç¥ UI

    [Header("Settings")]
    [SerializeField] private float fadeSpeed = 2f;

    private int currentUpgradePoint;

    private void OnEnable()
    {
        EventBus.Subscribe<UpgradePointChangedEvent>(OnUpgradePointChanged);
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<UpgradePointChangedEvent>(OnUpgradePointChanged);
    }

    private void Start()
    {
        exclamationUI.alpha = 0f;
    }

    private void Update()
    {
        HandleExclamation();
    }

    private void OnUpgradePointChanged(UpgradePointChangedEvent evt)
    {
        currentUpgradePoint = evt.Point;
    }

    private void HandleExclamation()
    {
        if (currentUpgradePoint > 0)
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
