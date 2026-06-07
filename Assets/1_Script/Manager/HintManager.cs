using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    [SerializeField] private HintView view;

    [Header("Settings")]
    [SerializeField] private float showDuration = 2f;

    private readonly Queue<HintEvent> queue = new();

    private bool isShowing;

    private readonly Dictionary<string, float> recentHints = new();

    private void OnEnable()
    {
        EventBus.Subscribe<HintEvent>(HandleHint);
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<HintEvent>(HandleHint);
    }

    private void HandleHint(HintEvent evt)
    {
        if (IsDuplicate(evt.Message))
            return;

        switch (evt.Type)
        {
            case HintType.Danger:

                StopAllCoroutines();

                StartCoroutine(
                    ShowDanger(evt));

                break;

            default:

                queue.Enqueue(evt);

                if (!isShowing)
                    StartCoroutine(ProcessQueue());

                break;
        }
    }

    private IEnumerator ProcessQueue()
    {
        isShowing = true;

        while (queue.Count > 0)
        {
            yield return ShowNormal(queue.Dequeue());
        }

        isShowing = false;
    }

    private IEnumerator ShowNormal(HintEvent evt)
    {
        TextMeshProUGUI text = view.HintText;
        SetupText(text, evt);
        yield return AnimateText(text);
    }

    private IEnumerator ShowDanger(HintEvent evt)
    {
        TextMeshProUGUI text = view.HintText;

        SetupText(text, evt);

        text.transform.DOShakeScale(0.4f, 0.3f);

        yield return AnimateText(text);
    }
   
    private IEnumerator AnimateText(TextMeshProUGUI text)
    {
        text.gameObject.SetActive(true);

        Color c = text.color;

        c.a = 0;

        text.color = c;

        text.DOFade(1f, 0.25f);

        yield return new WaitForSeconds(showDuration);

        yield return text.DOFade(0f, 0.25f).WaitForCompletion();

        text.gameObject.SetActive(false);
    }

    private void SetupText(TextMeshProUGUI text, HintEvent evt)
    {
        text.text = evt.Message;

        switch (evt.Type)
        {
            case HintType.Info:
                text.color = Color.white;
                break;

            case HintType.Warning:
                text.color = Color.yellow;
                break;

            case HintType.Danger:
                text.color = Color.red;
                break;

            case HintType.Discovery:
                text.color = Color.cyan;
                break;
        }
    }

    // 복제된 메시지인지 확인하는 함수   
    private bool IsDuplicate(string message)
    {
        float currentTime = Time.time;

        if (recentHints.TryGetValue(message, out float lastTime))
        {
            if (currentTime - lastTime < 3f)
                return true;
        }

        recentHints[message] = currentTime;

        return false;
    }
}
