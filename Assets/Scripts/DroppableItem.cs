using System.Collections;
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
    public AudioClip pickupSound; // Sound to play when the item is picked up

    private AudioSource audioSource;

    private void Start()
    {
        // Ensure the AudioSource component is attached
        audioSource = gameObject.GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false; // Ensure Play On Awake is disabled
        }

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
                PlayPickupSound(); // Play the pickup sound effect
                Destroy(gameObject); // Destroy the item immediately
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

    private void PlayPickupSound()
    {
        if (pickupSound != null)
        {
            GameObject soundGameObject = new GameObject("PickupSound");
            AudioSource newAudioSource = soundGameObject.AddComponent<AudioSource>();
            newAudioSource.clip = pickupSound;
            newAudioSource.Play();

            // Destroy the sound GameObject after the sound has finished playing
            Destroy(soundGameObject, pickupSound.length);
            Debug.Log("Pickup sound played.");
        }
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
