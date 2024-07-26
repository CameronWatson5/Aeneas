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
    public float lifetime = 10f; // Time in seconds before the item is destroyed if not collected

    private void Start()
    {
        // Start the coroutine to destroy the item after a set time
        StartCoroutine(DestroyAfterTime());
    }

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
                playerAttributes.Heal(amount);
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

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}