using UnityEngine;

[System.Serializable]
public class ShopItem
{
    public string itemName;
    public Sprite icon;
    public int price;
    public int damageBonus;
    public int armorBonus;
    public int healthBonus;
    public ItemType itemType; 
}