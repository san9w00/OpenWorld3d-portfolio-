using UnityEngine;

public class RequiredResourceViewModel
{
    public string ItemName;
    public int NeedAmount;

    private ItemSO item;
    public int CurrentAmount => PlayerInventory.Instance.GetItemCount(item);

    public bool IsEnough => CurrentAmount >= NeedAmount;

    public RequiredResourceViewModel(RequiredResource data)
    {
        item = data.item;
        ItemName = data.item.itemName;
        NeedAmount = data.amount;
    }
}
