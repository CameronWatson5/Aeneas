// This is used to set up the player's inventory and the max size of the inventory.

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

    private bool startingItemsEquipped = false;

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

    private void Start()
    {
        AddStartingItems();
        ApplyAllEquippedItemEffects();
    }

    void AddStartingItems()
    {
        foreach (var item in startingItems)
        {
            AddItem(item);
        }
    }

    void TryEquipStartingItems()
    {
        EnsurePlayerAttributes();
        if (playerAttributes != null && !startingItemsEquipped)
        {
            foreach (var item in startingItems)
            {
                EquipItem(item);
            }
            startingItemsEquipped = true;
            CancelInvoke(nameof(TryEquipStartingItems));
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
        if (playerAttributes == null)
        {
            Debug.LogWarning("Player attributes not found, cannot equip item.");
            return;
        }

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
        if (playerAttributes == null)
        {
            Debug.LogWarning("Player attributes not found, cannot unequip item.");
            return;
        }

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

            Debug.Log($"Unequipped item: {itemToUnequip.itemName}");
        }
    }

    private void ApplyItemEffects(InventoryItem item, bool apply)
    {
        EnsurePlayerAttributes();
        if (playerAttributes == null)
        {
            Debug.LogWarning("Player attributes not found, cannot apply item effects.");
            return;
        }

        int multiplier = apply ? 1 : -1;
        Debug.Log($"Applying item effects: {item.itemName}, Damage: {item.damageBonus * multiplier}, Armor: {item.armorBonus * multiplier}, Health: {item.healthBonus * multiplier}");
        playerAttributes.IncreaseDamage(item.damageBonus * multiplier);
        playerAttributes.IncreaseArmor(item.armorBonus * multiplier);
        playerAttributes.IncreaseHealth(item.healthBonus * multiplier);
        Debug.Log($"Player stats after applying item: Damage={playerAttributes.damage}, Armor={playerAttributes.armor}, Health={playerAttributes.maxHealth}");
    }

    public void ApplyAllEquippedItemEffects()
    {
        foreach (var item in inventory)
        {
            if (item.isEquipped)
            {
                ApplyItemEffects(item, true);
            }
        }
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
                Debug.LogWarning("Player not found in the scene!");
            }
        }
    }

    public void ReapplyEquippedItemEffects()
    {
        EnsurePlayerAttributes();
        if (playerAttributes == null)
        {
            Debug.LogWarning("Player attributes not found, cannot reapply item effects.");
            return;
        }

        foreach (var item in inventory)
        {
            if (item.isEquipped)
            {
                ApplyItemEffects(item, true);
            }
        }
    }
}
