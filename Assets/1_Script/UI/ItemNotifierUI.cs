using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemNotifierUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text itemName;

    private CanvasGroup canvasGroup;

    [Header("Fade")]
    [SerializeField] private float fadeTime = 0.3f;
    [SerializeField] private float stayTime = 2f;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Setup(ItemSO item, int amount)
    {
        icon.sprite = item.itemIcon;

        itemName.text = $"{item.itemName}";

        StartCoroutine(Play());
    }

    private IEnumerator Play()
    {
        yield return Fade(0, 1);

        yield return new WaitForSeconds(stayTime);

        yield return Fade(1, 0);

        Destroy(gameObject);
    }

    private IEnumerator Fade(float start, float end)
    {
        float time = 0f;

        while (time < fadeTime)
        {
            time += Time.deltaTime;

            canvasGroup.alpha = Mathf.Lerp(start, end, time / fadeTime);

            yield return null;
        }

        canvasGroup.alpha = end;
    }
}
