using UnityEngine;
using System.Collections.Generic;

// ItemID -> ItemSO 찾아주는 시스템.
public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance;

    [SerializeField] private List<ItemSO> allItems;

    private Dictionary<string, ItemSO> itemDict = new();

    private void Awake()
    {
        Instance = this;

        foreach (var item in allItems)
        {
            if (!itemDict.ContainsKey(item.itemID))
            {
                itemDict.Add(item.itemID, item);
            }
        }
    }

    public ItemSO GetItem(string id)
    {
        return itemDict.TryGetValue(id, out var item) ? item : null;
    }
}
