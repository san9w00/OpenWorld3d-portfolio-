using UnityEngine;

[CreateAssetMenu(menuName = "Data/Shop")]
public class ShopData : ScriptableObject
{
    public ItemSO[] items = new ItemSO[3];
}
