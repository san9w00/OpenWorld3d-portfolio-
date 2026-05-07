using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    public static CraftingSystem Instance;

    private void Awake()
    {
        Instance = this;
    }

    public bool CanCraft(CraftRecipeSO recipe)
    {
        foreach (var need in recipe.requiredResources)
        {
            int currentAmount = PlayerInventory.Instance.GetItemCount(need.item);

            if (currentAmount < need.amount)
            {
                return false;
            }
        }

        return true;
    }

    public void Craft(CraftRecipeSO recipe)
    {
        if (!CanCraft(recipe))
        {
            Debug.Log("재료 부족");
            return;
        }

        // 재료 차감
        foreach (var need in recipe.requiredResources)
        {
            PlayerInventory.Instance.RemoveItem(need.item, need.amount);
        }

        // 결과 지급
        PlayerInventory.Instance.AddItem(recipe.resultItem);

        Debug.Log(recipe.resultItem.itemName + " 제작 완료");
    }
}
