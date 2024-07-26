using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string itemName;
    public ItemType itemType;
    public bool isEquipped;
    public Sprite icon;
    public int damageBonus; 
    public int armorBonus; 
    public int healthBonus; 
}

public enum ItemType
{
    Sword,
    Shield,
    Helmet,
    Boots
}