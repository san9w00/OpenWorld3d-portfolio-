using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class AreaTextUIManager : MonoBehaviour
{
    public static AreaTextUIManager Instance;

    [SerializeField] private CanvasGroup panel;
    [SerializeField] private TMP_Text titleText;

    [SerializeField] private float fadeTime = 2f;
    [SerializeField] private float stayTime = 3f;

    private Coroutine currentRoutine;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        panel.alpha = 0;
    }

    public void ShowLocation(string title)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowRoutine(title));
    }

    private IEnumerator ShowRoutine(string title)
    {
        titleText.text = title;

        yield return Fade(0, 1);

        yield return new WaitForSeconds(stayTime);

        yield return Fade(1, 0);
    }

    private IEnumerator Fade(float from, float to)
    {
        float t = 0;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            panel.alpha = Mathf.Lerp(from, to, t / fadeTime);
            yield return null;
        }

        panel.alpha = to;
    }
}
