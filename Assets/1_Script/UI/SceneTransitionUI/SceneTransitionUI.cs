using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneTransitionUI : MonoBehaviour
{
    public static SceneTransitionUI Instance;

    [Header("Canvas")]
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("UI")]
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private TMP_Text tipText;

    [Header("Tips")]
    [TextArea]
    [SerializeField] private string[] tips;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        canvasGroup.alpha = 0;
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);

        loadingSlider.value = 0;

        tipText.text = tips[Random.Range(0, tips.Length)];

        StartCoroutine(LoadingTextAnimation());
    }

    public void Hide()
    {
        StopAllCoroutines();

        gameObject.SetActive(false);
    }

    public void SetProgress(float value)
    {
        loadingSlider.value = value;
    }

    public IEnumerator FadeIn(float duration)
    {
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;

            canvasGroup.alpha = Mathf.Lerp(0, 1, time / duration);

            yield return null;
        }

        canvasGroup.alpha = 1;
    }

    public IEnumerator FadeOut(float duration)
    {
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;

            canvasGroup.alpha = Mathf.Lerp(1, 0, time / duration);

            yield return null;
        }

        canvasGroup.alpha = 0;
    }

    private IEnumerator LoadingTextAnimation()
    {
        while (true)
        {
            loadingText.text = "Loading.";
            yield return new WaitForSeconds(0.3f);

            loadingText.text = "Loading..";
            yield return new WaitForSeconds(0.3f);

            loadingText.text = "Loading...";
            yield return new WaitForSeconds(0.3f);
        }
    }
}
