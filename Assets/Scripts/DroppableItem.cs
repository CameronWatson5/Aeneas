using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppableItem : MonoBehaviour
{
    public enum ItemType
    {
        Health,
        Damage,
        Armor,
        Gold
    }

    public ItemType itemType;
    public int amount;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AeneasAttributes playerAttributes = other.GetComponent<AeneasAttributes>();
            if (playerAttributes != null)
            {
                ApplyEffect(playerAttributes);
                Destroy(gameObject);
            }
        }
    }

    private void ApplyEffect(AeneasAttributes playerAttributes)
    {
        switch (itemType)
        {
            case ItemType.Health:
                playerAttributes.IncreaseHealth(amount);
                break;
            case ItemType.Damage:
                playerAttributes.IncreaseDamage(amount);
                break;
            case ItemType.Armor:
                playerAttributes.IncreaseArmor(amount);
                break;
            case ItemType.Gold:
                playerAttributes.AddGold(amount);
                break;
        }
    }
}