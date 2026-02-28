using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI explainText;

    private ItemSO currentItem;

    public void SetItem(ItemSO item)
    {
        currentItem = item;
        icon.sprite = item.itemIcon;
        nameText.text = item.itemName;
        priceText.text = item.price.ToString();
        explainText.text = item.itemExplain;
    }

    public void OnClickBuy()
    {
        if (currentItem == null) return;

        PlayerInventory inventory = FindAnyObjectByType<PlayerInventory>();
        PlayerGold gold = FindAnyObjectByType<PlayerGold>();

        if (gold == null || inventory == null) return;

        // 골드 검사, 차감
        if (!gold.TrySpendGold(currentItem.price))
        {
            Debug.Log("골드가 부족하다!");
            return;
        }

        // 인벤토리 추가
        inventory.AddItem(currentItem, 1);

        Debug.Log($"{currentItem.itemName} 구매완료! 이용해주셔서 감사합니다.");
    }

    public void Clear()
    {
        currentItem = null;
        icon.sprite = null;
        nameText.text = "";
        priceText.text = "";
        explainText.text = "";
    }
}
