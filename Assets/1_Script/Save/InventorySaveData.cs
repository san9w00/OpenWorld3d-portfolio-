using System;

// 인벤토리에서 가지고있는 SO는 저장이 안되므로 ID로 저장

[Serializable]
public class InventorySaveData
{
    public string itemID;
    public int quantity;
}
