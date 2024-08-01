using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    private string savePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            savePath = Application.persistentDataPath + "/save";
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame(int slot)
    {
        string path = savePath + slot + ".sav";
        SaveData data = CreateSaveData();
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
        Debug.Log("Game saved in slot " + slot);
    }

    public SaveData LoadGame(int slot)
    {
        string path = savePath + slot + ".sav";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Game loaded from slot " + slot);
            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found in slot " + slot);
            return null;
        }
    }

    public bool SaveFileExists(int slot)
    {
        string path = savePath + slot + ".sav";
        return File.Exists(path);
    }

    private SaveData CreateSaveData()
    {
        SaveData data = new SaveData();
        // Collect the game data to be saved
        var player = FindObjectOfType<AeneasAttributes>();
        if (player != null)
        {
            data.playerPosition = player.transform.position;
            data.health = player.currentHealth;
            data.gold = player.gold;
            // Add other necessary data

            // Collect inventory data
            data.inventory = InventoryManager.Instance.inventory;
            // Collect other necessary data
        }
        return data;
    }
}