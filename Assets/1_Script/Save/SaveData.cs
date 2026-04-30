using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    // Scene
    public string currentScene;

    // Player Position
    public float posX;
    public float posY;
    public float posZ;

    // Campfire
    public string currentCampFireID;
    public List<string> activatedCampfires = new();

    // PlayerStatus
    public int gold;

    // Level
    public int level;
    public int exp;
    public int upgradePoint;

    // Inventory
    public List<InventorySaveData> inventoryItems = new();
}
