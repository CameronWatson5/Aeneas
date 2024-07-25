using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public List<InventoryItem> inventory = new List<InventoryItem>();
    public int inventorySize = 20;
    public InventoryItem[] startingItems;

    private InventoryItem equippedSword;
    private InventoryItem equippedShield;
    private InventoryItem equippedHelmet;
    private InventoryItem equippedBoots;
    private AeneasAttributes playerAttributes;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        AddStartingItems();
    }

    void AddStartingItems()
    {
        foreach (var item in startingItems)
        {
            AddItem(item);
        }
    }

    public bool AddItem(InventoryItem item)
    {
        if (inventory.Count < inventorySize)
        {
            inventory.Add(item);
            return true;
        }
        return false;
    }

    public void RemoveItem(InventoryItem item)
    {
        inventory.Remove(item);
    }

    public InventoryItem GetEquippedItem(ItemType itemType)
    {
        return inventory.Find(item => item.itemType == itemType && item.isEquipped);
    }

    public void RearrangeInventory(int fromIndex, int toIndex)
    {
        if (fromIndex >= 0 && fromIndex < inventory.Count && toIndex >= 0 && toIndex < inventory.Count)
        {
            InventoryItem item = inventory[fromIndex];
            inventory.RemoveAt(fromIndex);
            inventory.Insert(toIndex, item);
        }
    }

    public void EquipItem(InventoryItem item)
    {
        EnsurePlayerAttributes();
        InventoryItem currentItem = GetEquippedItem(item.itemType);
        if (currentItem != null)
        {
            currentItem.isEquipped = false;
            ApplyItemEffects(currentItem, false); // Remove effects
        }

        switch (item.itemType)
        {
            case ItemType.Sword:
                equippedSword = item;
                break;
            case ItemType.Shield:
                equippedShield = item;
                break;
            case ItemType.Helmet:
                equippedHelmet = item;
                break;
            case ItemType.Boots:
                equippedBoots = item;
                break;
        }

        item.isEquipped = true;
        ApplyItemEffects(item, true); // Apply effects
    }

    public void UnequipItem(ItemType itemType)
    {
        EnsurePlayerAttributes();
        InventoryItem itemToUnequip = GetEquippedItem(itemType);
        if (itemToUnequip != null)
        {
            itemToUnequip.isEquipped = false;
            ApplyItemEffects(itemToUnequip, false); // Remove effects

            switch (itemType)
            {
                case ItemType.Sword:
                    equippedSword = null;
                    break;
                case ItemType.Shield:
                    equippedShield = null;
                    break;
                case ItemType.Helmet:
                    equippedHelmet = null;
                    break;
                case ItemType.Boots:
                    equippedBoots = null;
                    break;
            }

            // Debugging logs
            Debug.Log($"Unequipped item: {itemToUnequip.itemName}");
        }
    }



    private void ApplyItemEffects(InventoryItem item, bool apply)
    {
        EnsurePlayerAttributes();
        int multiplier = apply ? 1 : -1;
        playerAttributes.IncreaseDamage(item.damageBonus * multiplier);
        playerAttributes.IncreaseArmor(item.armorBonus * multiplier);
        playerAttributes.IncreaseHealth(item.healthBonus * multiplier);
    }

    private void EnsurePlayerAttributes()
    {
        if (playerAttributes == null)
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
    }
}
