using System.Collections;
using TMPro;
using UnityEngine;

public class LevelUPMiddleTextUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI levelUpText;
    [SerializeField] private TextMeshProUGUI lvText;

    private Coroutine currentRoutine;

    private void OnEnable()
    {
        EventBus.Subscribe<LevelUpEvent>(OnLevelChanged);
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<LevelUpEvent>(OnLevelChanged);
    }

    private void Start()
    {
        canvasGroup.alpha = 0;
    }

    private void OnLevelChanged(LevelUpEvent evt)
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
        }

        levelUpText.text = "LEVEL UP";

        lvText.text = $"Lv.{evt.NewLevel}";

        currentRoutine =
            StartCoroutine(LevelUpAnimation());
    }

    private IEnumerator LevelUpAnimation()
    {
        canvasGroup.alpha = 0;

        // Fade In
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * 2f;

            canvasGroup.alpha =
                Mathf.Lerp(0, 1, t);

            yield return null;
        }

        yield return new WaitForSeconds(4f);

        // Fade Out
        t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * 2f;

            canvasGroup.alpha =
                Mathf.Lerp(1, 0, t);

            yield return null;
        }
    }
}
