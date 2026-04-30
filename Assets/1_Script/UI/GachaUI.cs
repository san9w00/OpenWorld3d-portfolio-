using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GachaUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Image resultImg;

    [Header("settings")]
    [SerializeField] private int gachaCost = 100;
    [SerializeField] private int coinRewardAmount = 150;

    [Header("Items")]
    [SerializeField] private ItemSO firstItem;
    [SerializeField] private ItemSO secondItem;
    [SerializeField] private Sprite coinSprite;

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
        InputHandler.Instance.SetInventoryState(isOpen);
    }

    public void Close()
    {
        panel.SetActive(false);
        InputHandler.Instance.SetInventoryState(false);
    }

    public void Roll()
    {
        if (!PlayerStatus.Instance.TrySpendGold(gachaCost))
        {
            Debug.Log("골드가 부족하다!");
            return;
        }

        int random = Random.Range(0, 100);

        if (random < 40)
        {
            PlayerInventory.Instance.AddItem(firstItem, 1);
            StartCoroutine(ShowResult(firstItem.itemIcon));
        }
        else if (random < 80)
        {
            PlayerStatus.Instance.AddGold(coinRewardAmount);
            StartCoroutine(ShowResult(coinSprite));
        }
        else
        {
            PlayerInventory.Instance.AddItem(secondItem, 1);
            StartCoroutine(ShowResult(secondItem.itemIcon));
        }
    }

    private IEnumerator ShowResult(Sprite icon)
    {
        resultImg.sprite = icon;

        resultImg.gameObject.SetActive(true);

        resultImg.transform.localScale = Vector3.one;

        yield return new WaitForSeconds(3f);

        resultImg.gameObject.SetActive(false);
    }
}
