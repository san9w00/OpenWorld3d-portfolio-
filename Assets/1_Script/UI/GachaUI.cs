using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GachaUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Image resultImg;

    [Header("settings")]
    [SerializeField] private int gachaCost = 300;

    [Header("Items")]
    [SerializeField] private ItemSO firstItem;
    [SerializeField] private ItemSO secondItem;
    [SerializeField] private ItemSO thirdItem;
    [SerializeField] private Sprite coinSprite;

    private bool isRolling;

    private void Start()
    {
        resultImg.gameObject.SetActive(false);
    }

    public void Open()
    {
        ToggleShopPanel();
    }

    void ToggleShopPanel()
    {
        bool isOpen = !panel.activeSelf;
        panel.SetActive(isOpen);

        if (isOpen)
        {
            UIManager.Instance.OpenUI(panel);
        }
        else
        {
            UIManager.Instance.CloseUI(panel);
        }
    }

    public void Close()
    {
        panel.SetActive(false);

        UIManager.Instance.CloseUI(panel);
    }

    public void Roll()
    {
        if (isRolling)
            return;

        if (!PlayerStatus.Instance.TrySpendGold(gachaCost))
        {
            Debug.Log("골드가 부족하다!");
            return;
        }

        isRolling = true;

        int random = Random.Range(0, 100);

        if (random < 60)
        {
            PlayerInventory.Instance.AddItem(firstItem, 1);
            StartCoroutine(ShowResult(firstItem.itemIcon));
        }
        else if (random < 95)
        {
            PlayerInventory.Instance.AddItem(secondItem, 1);
            StartCoroutine(ShowResult(secondItem.itemIcon));
        }
        else
        {
            PlayerInventory.Instance.AddItem(thirdItem, 1);
            StartCoroutine(ShowResult(thirdItem.itemIcon));
        }
    }

    private IEnumerator ShowResult(Sprite icon)
    {
        resultImg.sprite = icon;
        resultImg.gameObject.SetActive(true);

        // 초기 상태
        resultImg.color = new Color(1, 1, 1, 1);
        resultImg.transform.localScale = Vector3.zero;

        // 뿅! 등장
        Sequence seq = DOTween.Sequence();

        seq.Append(resultImg.transform.DOScale(1.4f, 0.25f).SetEase(Ease.OutBack));

        seq.Append(resultImg.transform.DOScale(1f, 0.15f).SetEase(Ease.OutQuad));

        yield return seq.WaitForCompletion();

        yield return new WaitForSeconds(2f);

        // 사라질 때
        Sequence hideSeq = DOTween.Sequence();

        hideSeq.Join(resultImg.transform.DOScale(1.3f, 0.3f));

        hideSeq.Join(resultImg.DOFade(0f, 0.3f));

        yield return hideSeq.WaitForCompletion();

        resultImg.gameObject.SetActive(false);

        isRolling = false;
    }
}
