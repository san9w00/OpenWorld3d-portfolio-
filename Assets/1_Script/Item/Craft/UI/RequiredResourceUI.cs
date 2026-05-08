using TMPro;
using UnityEngine;

public class RequiredResourceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI amountText;

    public void Setup(
        RequiredResourceViewModel vm)
    {
        itemNameText.text = vm.ItemName;

        amountText.text =
            $"{vm.CurrentAmount} / {vm.NeedAmount}";

        amountText.color =
            vm.IsEnough
            ? Color.white
            : Color.red;
    }
}
