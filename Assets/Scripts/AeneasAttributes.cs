// This script keeps track of the player's statistics, such as health, damage, armor, gold, and speed.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AeneasAttributes : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int damage = 10;
    public int armor = 0;
    public float speed = 5.0f;
    public int gold = 0;
    private const int maxGold = 999;

    private void Start()
    {
        ResetAttributes();
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.ApplyAllEquippedItemEffects();
        }
        else
        {
            Debug.LogError("InventoryManager not found!");
        }
    }

    public void TakeDamage(int amount)
    {
        int damageToTake = amount - armor;
        damageToTake = Mathf.Clamp(damageToTake, 0, int.MaxValue);
        currentHealth -= damageToTake;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void IncreaseHealth(int amount)
    {
        maxHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }

    public void IncreaseDamage(int amount)
    {
        damage += amount;
    }

    public void IncreaseArmor(int amount)
    {
        armor += amount;
    }

    public void AddGold(int amount)
    {
        gold += amount;
        gold = Mathf.Clamp(gold, 0, maxGold);
    }

    public bool SpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            return true;
        }
        return false;
    }

    public void Die()
    {
        Debug.Log("Player has died");
        GameManager.Instance.PlayerDied();
    }

    public void ResetAttributes()
    {
        currentHealth = maxHealth;
        damage = 10;
        armor = 0;
        speed = 5.0f;
        gold = 0;
        Debug.Log("Player attributes have been reset.");
    }
}