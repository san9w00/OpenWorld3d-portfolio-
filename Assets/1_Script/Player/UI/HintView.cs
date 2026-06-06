using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Collections;

public class HintView : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI hintText;

    [Header("Settings")]
    [SerializeField] private float showTime = 2.5f;

    private HintPresenter presenter;

    private readonly Queue<HintData> hintQueue = new();

    private bool isShowing;

    private void Awake()
    {
        presenter = GetComponent<HintPresenter>();
    }

    private void OnEnable()
    {
        presenter.OnHintReceived += HandleHint;
    }

    private void OnDisable()
    {
        presenter.OnHintReceived -= HandleHint;
    }

    private void HandleHint(HintData hint)
    {
        hintQueue.Enqueue(hint);

        if (!isShowing)
            StartCoroutine(ProcessQueue());
    }

    private IEnumerator ProcessQueue()
    {
        isShowing = true;

        while (hintQueue.Count > 0)
        {
            HintData hint = hintQueue.Dequeue();

            ShowHint(hint);

            yield return new WaitForSeconds(showTime);
        }

        hintText.gameObject.SetActive(false);

        isShowing = false;
    }

    private void ShowHint(HintData hint)
    {
        hintText.gameObject.SetActive(true);

        hintText.text = hint.Message;

        switch (hint.Type)
        {
            case HintType.Info:
                hintText.color = Color.white;
                break;

            case HintType.Warning:
                hintText.color = Color.yellow;
                break;

            case HintType.Danger:
                hintText.color = Color.red;
                break;

            case HintType.Achievement:
                hintText.color = new Color(1f, 0.8f, 0f);
                break;
        }
    }
}
