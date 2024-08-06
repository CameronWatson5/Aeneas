using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Main UI References")]
    public Canvas pauseMenuCanvas;
    public GameObject sideNavigationBar; // Reference to the side navigation bar
    public Button resumeButton;
    public Button quitButton;

    [Header("Panels")]
    //public GameObject savePanel;
    //public GameObject loadPanel;

    [Header("Save/Load Slots")]
    //public Button[] saveSlots; // Buttons for save slots
    //public Button[] loadSlots; // Buttons for load slots

    [Header("Inventory")]
    public Transform inventoryContent;
    public GameObject inventoryItemPrefab;

    [Header("Equipment Slots")]
    public Image swordSlot;
    public Image shieldSlot;
    public Image helmetSlot;
    public Image bootsSlot;
    public TMP_Text swordText;
    public TMP_Text shieldText;
    public TMP_Text helmetText;
    public TMP_Text bootsText;

    [Header("Log Management")]
    public Transform logContent; // The Transform that will hold log entries
    public GameObject logEntryPrefab; // Prefab for log entries

    [Header("Map Management")]
    public Image mapImage; 
    public Sprite defaultMapSprite;

    private InventoryManager inventoryManager;
    private string previousScene;

    void Start()
    {
        InitializePauseMenu();
        SetupButtonListeners();
        PopulateInventory();
        UpdateEquipmentDisplay();

        // Ensure side navigation bar is active
        if (sideNavigationBar != null)
        {
            sideNavigationBar.SetActive(true);
            Debug.Log("Side Navigation Bar is active.");
        }
        else
        {
            Debug.LogWarning("Side Navigation Bar reference is missing.");
        }

        // Initialize the map image
        LoadMapImage();
    }

    void InitializePauseMenu()
    {
        previousScene = PlayerPrefs.GetString("PreviousScene");

        if (pauseMenuCanvas == null)
            Debug.LogError("PauseMenuCanvas is not assigned in the Inspector.");

        inventoryManager = InventoryManager.Instance;
        if (inventoryManager == null)
            Debug.LogError("InventoryManager not found. Ensure it exists in the scene and is marked DontDestroyOnLoad.");

        if (inventoryContent == null)
            Debug.LogError("Inventory Content is not assigned. This should be the Content of your Inventory Scroll View.");

        // Initialize LogManager with log content and prefab
        if (LogManager.Instance != null)
        {
            LogManager.Instance.SetLogContent(logContent);
            LogManager.Instance.SetLogEntryPrefab(logEntryPrefab);
        }
        else
        {
            Debug.LogError("LogManager instance not found.");
        }

        // Hide save/load panels initially
        //savePanel.SetActive(false);
        //loadPanel.SetActive(false);
    }

    void SetupButtonListeners()
    {
        SetupButton(resumeButton, ResumeGame, "ResumeButton");
        SetupButton(quitButton, QuitGame, "QuitButton");

        // for (int i = 0; i < saveSlots.Length; i++)
        // {
        //     int slot = i; // Slots are 0-based
        //     saveSlots[i].onClick.AddListener(() => SaveGame(slot));
        //     loadSlots[i].onClick.AddListener(() => LoadGame(slot));
        // }
    }

    void SetupButton(Button button, UnityEngine.Events.UnityAction action, string buttonName)
    {
        if (button != null)
            button.onClick.AddListener(action);
        else
            Debug.LogError($"{buttonName} is not assigned in the Inspector.");
    }

    void PopulateInventory()
    {
        if (inventoryManager == null || inventoryContent == null || inventoryItemPrefab == null)
        {
            Debug.LogError("Inventory components are not properly set up.");
            return;
        }

        // Clear the existing inventory items
        foreach (Transform child in inventoryContent)
        {
            Destroy(child.gameObject);
        }

        // Populate the inventory with unequipped items
        foreach (InventoryItem item in inventoryManager.inventory)
        {
            Debug.Log($"Item in inventory: {item.itemName}, Equipped: {item.isEquipped}");

            if (!item.isEquipped)
            {
                GameObject itemObj = Instantiate(inventoryItemPrefab, inventoryContent);
                ItemUI itemUI = itemObj.GetComponent<ItemUI>();
                if (itemUI != null)
                {
                    itemUI.SetupItem(item, OnItemClicked);
                }
                else
                {
                    Debug.LogError("ItemUI component not found on the instantiated prefab.");
                }
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(inventoryContent.GetComponent<RectTransform>());
    }

    void OnItemClicked(InventoryItem item)
    {
        Debug.Log($"Item clicked: {item.itemName}, Equipped: {item.isEquipped}");

        if (item.isEquipped)
            inventoryManager.UnequipItem(item.itemType);
        else
            inventoryManager.EquipItem(item);

        UpdateEquipmentDisplay();
        PopulateInventory();
    }

    void UnequipItem(ItemType itemType)
    {
        inventoryManager.UnequipItem(itemType);
        UpdateEquipmentDisplay();
        PopulateInventory();
    }

    void EquipItem(ItemType itemType)
    {
        InventoryItem itemToEquip = inventoryManager.inventory.Find(item => item.itemType == itemType && !item.isEquipped);
        if (itemToEquip != null)
        {
            inventoryManager.EquipItem(itemToEquip);
            UpdateEquipmentDisplay();
            PopulateInventory();
        }
    }

    void UpdateEquipmentDisplay()
    {
        UpdateEquipmentSlot(swordSlot, swordText, ItemType.Sword);
        UpdateEquipmentSlot(shieldSlot, shieldText, ItemType.Shield);
        UpdateEquipmentSlot(helmetSlot, helmetText, ItemType.Helmet);
        UpdateEquipmentSlot(bootsSlot, bootsText, ItemType.Boots);
    }

    void UpdateEquipmentSlot(Image slotImage, TMP_Text slotText, ItemType itemType)
    {
        InventoryItem equippedItem = inventoryManager.GetEquippedItem(itemType);
        if (equippedItem != null)
        {
            slotImage.sprite = equippedItem.icon;
            slotText.text = equippedItem.itemName;
            slotImage.color = Color.white;
        }
        else
        {
            slotImage.sprite = null;
            slotText.text = "Empty";
            slotImage.color = new Color(1, 1, 1, 0);
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        if (SceneManager.GetSceneByName("PauseMenu").isLoaded)
        {
            SceneManager.UnloadSceneAsync("PauseMenu");
        }

        // Ensure the player and tilemap are correctly set after resuming
        CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
        if (cameraFollow != null)
        {
            cameraFollow.FindPlayer();
            cameraFollow.FindTilemap();
            cameraFollow.CalculateBounds();
            cameraFollow.isNewScene = true; // Force camera to center on player
        }

        PauseMenu.GameIsPaused = false; // Update the GameIsPaused state
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    void LoadMapImage()
    {
        string sceneName = PlayerPrefs.GetString("PreviousScene");
        MapManager mapManager = FindObjectOfType<MapManager>();
        if (mapManager != null)
        {
            mapManager.SetupMapForScene(sceneName);
        }
        else
        {
            Debug.LogError("MapManager not found in the scene.");
        }
    }

    void SaveGame(int slot)
    {
        SaveManager.Instance.SaveGame(slot);
        UpdateSlotInteractability();
    }

    void LoadGame(int slot)
    {
        SaveData data = SaveManager.Instance.LoadGame(slot);
        if (data != null)
        {
            // Apply loaded data to the game state
            ApplySaveData(data);
            ResumeGame();
        }
    }

    void UpdateSlotInteractability()
    {
        // for (int i = 0; i < saveSlots.Length; i++)
        // {
        //     int slot = i;
        //     saveSlots[i].interactable = SaveManager.Instance.SaveFileExists(slot);
        //     loadSlots[i].interactable = SaveManager.Instance.SaveFileExists(slot);
        // }
    }

    void ApplySaveData(SaveData data)
    {
        // Apply the loaded data to the game objects
        var player = FindObjectOfType<AeneasAttributes>();
        if (player != null)
        {
            player.transform.position = data.playerPosition;
            player.currentHealth = data.health;
            player.gold = data.gold;
            // Apply other saved attributes as needed

            // Update inventory
            inventoryManager.inventory = data.inventory;
            // Update quests or other game state data
        }
        else
        {
            Debug.LogError("Player object not found when trying to apply save data.");
        }
    }
}
