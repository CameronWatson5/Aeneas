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
    }

    void SetupButtonListeners()
    {
        SetupButton(resumeButton, ResumeGame, "ResumeButton");
        SetupButton(quitButton, QuitGame, "QuitButton");

        SetupEquipmentSlotButton(swordSlot, ItemType.Sword);
        SetupEquipmentSlotButton(shieldSlot, ItemType.Shield);
        SetupEquipmentSlotButton(helmetSlot, ItemType.Helmet);
        SetupEquipmentSlotButton(bootsSlot, ItemType.Boots);
    }

    void SetupButton(Button button, UnityEngine.Events.UnityAction action, string buttonName)
    {
        if (button != null)
            button.onClick.AddListener(action);
        else
            Debug.LogError($"{buttonName} is not assigned in the Inspector.");
    }

    void SetupEquipmentSlotButton(Image slotImage, ItemType itemType)
    {
        Button slotButton = slotImage.GetComponentInChildren<Button>();
        if (slotButton != null)
            slotButton.onClick.AddListener(() => UnequipItem(itemType));
        else
            Debug.LogError($"Button not found in {itemType} slot.");
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
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
