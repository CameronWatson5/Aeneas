using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public Vector2 playerPosition;
    public int health;
    public int gold;
    public List<InventoryItem> inventory;
    public List<string> completedQuests;
}

