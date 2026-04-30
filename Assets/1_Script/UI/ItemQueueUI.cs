using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemQueueUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemName;

    private PlayerInventory inventory;

    private CanvasGroup canvasGroup;

    private Queue<ItemSO> itemQueue = new Queue<ItemSO>();

    private bool isPlaying = false;

    float fadeTime = 0.3f;
    float stayTime = 2f;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        inventory = PlayerInventory.Instance;

        if (inventory != null)
        {
            inventory.OnItemAdded += AddQueue;
        }
        else
        {
            Debug.LogError("PlayerInventory Instance ¾øÀ½");
        }
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

    private void OnDestroy()
    {
        if (inventory != null)
        {
            inventory.OnItemAdded -= AddQueue;
        }
    }
}
