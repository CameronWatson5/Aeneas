// This script is used to manage the shop and store information about the items which are sold.

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    [Header("Shop Items")]
    public List<ShopItem> shopItems = new List<ShopItem>();

    [Header("UI References")]
    public Canvas shopCanvas;
    public GameObject shopPanel;
    public TextMeshProUGUI shopTitleText;
    public Transform itemListContent;
    public TextMeshProUGUI playerGoldText;
    public Button exitButton;
    public GameObject itemPrefab;
    public GameObject warningPanel; // Added this line
    public TextMeshProUGUI warningText; // Added this line

    [Header("Player References")]
    private AeneasAttributes playerAttributes;

    private InventoryManager inventoryManager;

    private bool isShopOpen = false;

    void Start()
    {
        FindPlayerAttributes();
        FindInventoryManager();
        InitializeShop();
        AdjustContentSize();
        if (itemListContent == null)
        {
            Debug.LogError("itemListContent is not assigned in ShopManager!");
        }
        else
        {
            Debug.Log($"itemListContent has {itemListContent.childCount} children");
        }
    }

    void AdjustContentSize()
    {
        RectTransform contentRect = itemListContent.GetComponent<RectTransform>();
        RectTransform itemRect = itemPrefab.GetComponent<RectTransform>();
        float totalHeight = shopItems.Count * (itemRect.rect.height + 10); // 10 is the spacing
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, totalHeight);
    }

    void FindPlayerAttributes()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerAttributes = player.GetComponent<AeneasAttributes>();
            if (playerAttributes == null)
            {
                Debug.LogError("AeneasAttributes component not found on the Player!");
            }
        }
        else
        {
            Debug.LogError("Player not found in the scene!");
        }
    }

    void FindInventoryManager()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager not found in the scene!");
        }
    }

    void InitializeShop()
    {
        if (playerAttributes == null)
        {
            playerAttributes = FindObjectOfType<AeneasAttributes>();
            if (playerAttributes == null)
            {
                Debug.LogError("Player Attributes not found!");
                return;
            }
        }

        if (shopCanvas == null)
        {
            Debug.LogError("Shop Canvas is not assigned!");
            return;
        }

        exitButton.onClick.AddListener(ToggleShop);
        shopPanel.SetActive(false); // Ensure the panel starts disabled
        PopulateShop();
        UpdatePlayerGoldDisplay();
    }

    public void ToggleShop()
    {
        isShopOpen = !isShopOpen;
        shopPanel.SetActive(isShopOpen);

        Debug.Log($"Shop toggled. IsShopOpen: {isShopOpen}");

        if (isShopOpen)
        {
            Time.timeScale = 0f; // Pause the game while shop is open
            UpdatePlayerGoldDisplay();
            Debug.Log("Shop opened and game paused");
            Debug.Log($"Shop panel active: {shopPanel.activeSelf}, Shop panel in hierarchy: {shopPanel.activeInHierarchy}");
            CheckItemVisibility();
        }
        else
        {
            Time.timeScale = 1f; // Resume the game when shop is closed
            Debug.Log("Shop closed and game resumed");
        }
    }

    void PopulateShop()
    {
        foreach (Transform child in itemListContent)
        {
            Destroy(child.gameObject);
        }

        foreach (ShopItem item in shopItems)
        {
            GameObject itemObj = Instantiate(itemPrefab, itemListContent);
            EquippableItemUI itemUI = itemObj.GetComponent<EquippableItemUI>();
            if (itemUI != null)
            {
                itemUI.SetupItem(item, BuyItem);
                Debug.Log($"Added item: {item.itemName} to shop at position {itemObj.transform.position}");
            }
            else
            {
                Debug.LogError("EquippableItemUI component not found on the instantiated prefab.");
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(itemListContent.GetComponent<RectTransform>());
    }

    void CheckItemVisibility()
    {
        foreach (RectTransform child in itemListContent)
        {
            EquippableItemUI itemUI = child.GetComponent<EquippableItemUI>();
            if (itemUI != null)
            {
                Vector3[] corners = new Vector3[4];
                child.GetWorldCorners(corners);
                bool isVisible = GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Camera.main), new Bounds(child.position, corners[2] - corners[0]));
                Debug.Log($"Item {itemUI.itemNameText.text} is visible: {isVisible}");
            }
        }
    }

    void BuyItem(ShopItem item)
    {
        if (playerAttributes.SpendGold(item.price))
        {
            InventoryItem newItem = ConvertShopItemToInventoryItem(item);

            if (inventoryManager.AddItem(newItem))
            {
                UpdatePlayerGoldDisplay();
                Debug.Log($"Bought {item.itemName} and added to inventory");

                // Remove the item from the shop inventory
                shopItems.Remove(item);
                PopulateShop(); // Refresh the shop UI
            }
            else
            {
                Debug.Log("Inventory is full");
                playerAttributes.AddGold(item.price); // Refund the gold
            }
        }
        else
        {
            Debug.Log("Not enough gold");
            StartCoroutine(ShowWarning("Not enough gold!"));
        }
    }

    IEnumerator ShowWarning(string message)
    {
        warningText.text = message;
        warningPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(2f); // Use WaitForSecondsRealtime to avoid issues with time scaling
        warningPanel.SetActive(false);
    }

    InventoryItem ConvertShopItemToInventoryItem(ShopItem shopItem)
    {
        return new InventoryItem
        {
            itemName = shopItem.itemName,
            icon = shopItem.icon,
            itemType = shopItem.itemType,
            damageBonus = shopItem.damageBonus,
            armorBonus = shopItem.armorBonus,
            healthBonus = shopItem.healthBonus,
            isEquipped = false // By default, newly bought items are not equipped
        };
    }

    void UpdatePlayerGoldDisplay()
    {
        playerGoldText.text = $"Gold: {playerAttributes.gold}";
    }
}
