using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AeneasAttributes : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int damage = 10;
    public int armor = 0;
    public float speed = 5.0f;
    public int gold = 0;

    void Start()
    {
        ResetAttributes();
        InventoryManager.Instance?.ReapplyEquippedItemEffects();
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

    private void Die()
    {
        Debug.Log("Player has died");
        SceneManager.LoadScene("GameOver");
    }

    public void ResetAttributes()
    {
        currentHealth = maxHealth;
        damage = 10;
        armor = 0;
        speed = 5.0f;
        gold = 0;
    }
}