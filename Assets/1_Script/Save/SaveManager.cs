using UnityEngine;
using System.IO;
using UnityEditor;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private string saveFolderPath;

    public int CurrentSlot { get; private set; } = -1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            saveFolderPath = Application.persistentDataPath + "/Saves/";

            if (!Directory.Exists(saveFolderPath))
            {
                Directory.CreateDirectory(saveFolderPath);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCurrentSlot(int slot)
    {
        CurrentSlot = slot;
    }

    string GetSavePath(int slot)
    {
        return saveFolderPath + $"Save_{slot}.json";
    }

    public bool HasSave(int slot)
    {
        return File.Exists(GetSavePath(slot));
    }

    public void SaveGame()
    {
        if (CurrentSlot < 0)
        {
            Debug.LogError("현재 슬롯이 선택되지 않았습니다.");
            return;
        }

        SaveData data = CreateSaveDate();

        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(GetSavePath(CurrentSlot), json);

        Debug.Log($"저장 완료 : Slot {CurrentSlot}");
    }

    public SaveData LoadGame(int slot)
    {
        string path = GetSavePath(slot);

        if (!File.Exists(path))
        {
            Debug.LogError("세이브 파일이 존재하지 않습니다.");
            return null;
        }

        string json = File.ReadAllText(path);

        SaveData data = JsonUtility.FromJson<SaveData>(json);

        CurrentSlot = slot;

        Debug.Log($"로드 완료 : Slot {slot}");

        return data;
    }

    SaveData CreateSaveDate()
    {
        SaveData data = new SaveData();

        // Scene
        data.currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        // Player Pos
        GameObject player = PlayerManager.Instance.Player;

        data.posX = player.transform.position.x;
        data.posY = player.transform.position.y;
        data.posZ = player.transform.position.z;

        // CampFire
        data.activatedCampfires = CampFireManager.Instance.GetAllActivated();

        // Gold
        PlayerStatus status = player.GetComponent<PlayerStatus>();
        data.gold = status.Gold;

        // Level
        PlayerLevelSystem level = player.GetComponent<PlayerLevelSystem>();

        data.level = level.CurrentLevel;
        data.exp = level.CurrentExp;
        data.upgradePoint = level.UpgradePoint;

        // Inventory
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();

        foreach (var item in inventory.items)
        {
            InventorySaveData saveItem = new InventorySaveData();

            saveItem.itemID = item.itemData.itemID;

            saveItem.quantity = item.quantity;

            data.inventoryItems.Add(saveItem);
        }

        return data;
    }
}
