using UnityEngine;

[CreateAssetMenu(menuName = "Items/Resource")]
public class ResourceItemSO : ItemSO
{
    [Header("Resource")]
    public ResourceType resourceType;

    public override void Use(GameObject player)
    {
        Debug.Log(itemName + " 은(는) 사용할 수 없는 자원 아이템입니다.");
    }
}
