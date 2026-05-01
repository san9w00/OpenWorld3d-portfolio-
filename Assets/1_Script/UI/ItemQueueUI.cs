using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemQueueUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemName;

    private CanvasGroup canvasGroup;

    private Queue<ItemSO> itemQueue = new Queue<ItemSO>();

    private bool isPlaying = false;

    float fadeTime = 0.3f;
    float stayTime = 2f;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        EventBus.Subscribe<ItemAddedEvent>(OnItemAddedMsg);
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<ItemAddedEvent>(OnItemAddedMsg);
    }

    // 이벤트 매개변수를 받는 래퍼 함수
    private void OnItemAddedMsg(ItemAddedEvent evt)
    {
        AddQueue(evt.Item, evt.Amount);
    }

    void AddQueue(ItemSO item, int amount)
    {
        itemQueue.Enqueue(item);

        if (!isPlaying)
            StartCoroutine(PlayQueue());
    }

    IEnumerator PlayQueue()
    {
        isPlaying = true;

        while (itemQueue.Count > 0)
        {
            ItemSO item = itemQueue.Dequeue();

            icon.sprite = item.itemIcon;
            itemName.text = item.itemName;

            yield return StartCoroutine(Fade(0, 1));

            yield return new WaitForSeconds(stayTime);

            yield return StartCoroutine(Fade(1, 0));
        }

        isPlaying = false;
    }

    IEnumerator Fade(float start, float end)
    {
        float time = 0;

        while (time < fadeTime)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, time / fadeTime);
            yield return null;
        }

        canvasGroup.alpha = end;
    }
}
