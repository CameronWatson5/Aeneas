// This script is used for the enemies' health.
// This script allows for enemies to be defeated.
// Furthermore, this script also controls the items which are dropped and the percentage chance
// that these items will be dropped. It also controls the hurt and death sound effects.

using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float knockbackDuration = 0.2f;
    [SerializeField] private GameObject[] droppableItems;
    [SerializeField] private float dropChance = 0.2f;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;

    public event System.Action<GameObject> OnEnemyDeath;

    public int CurrentHealth
    {
        get { return _currentHealth; }
        private set { _currentHealth = Mathf.Clamp(value, 0, maxHealth); }
    }

    public bool IsDead { get; private set; }

    private int _currentHealth;
    private Rigidbody2D rb;
    private AudioSource audioSource;

    void Start()
    {
        CurrentHealth = maxHealth;
        IsDead = false;
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        if (rb != null)
        {
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb.drag = 0;
        }

        if (audioSource == null)
        {
            Debug.LogError("EnemyHealth: AudioSource component missing on the enemy!");
        }

        EnemyManager.Instance.RegisterEnemy(gameObject);
    }

    private void OnDestroy()
    {
        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.UnregisterEnemy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        if (IsDead) return;

        CurrentHealth -= damage;
        Debug.Log($"Enemy took {damage} damage. Current health: {CurrentHealth}");

        if (CurrentHealth <= 0)
        {
            Die();
        }
        else
        {
            if (hurtSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(hurtSound);
            }
        }
    }

    public void ApplyKnockback(Vector2 knockbackDirection, float knockbackForce)
    {
        if (IsDead) return;
        
        Debug.Log("Enemy should be knocked back");
        StartCoroutine(ApplyKnockbackCoroutine(knockbackDirection, knockbackForce));
    }

    private void Die()
    {
        if (IsDead) return;

        IsDead = true;
        Debug.Log("Enemy died");

        // Immediately disable collider
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        // Disable the renderer
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = false;
        }

        // Disable the EnemyController
        EnemyController controller = GetComponent<EnemyController>();
        if (controller != null)
        {
            controller.enabled = false;
        }

        OnEnemyDeath?.Invoke(gameObject);
        DropItem();
        PlayDeathSound();

        Destroy(gameObject);
    }

    private void DropItem()
    {
        if (droppableItems.Length > 0 && Random.value <= dropChance)
        {
            int randomIndex = Random.Range(0, droppableItems.Length);
            Instantiate(droppableItems[randomIndex], transform.position, Quaternion.identity);
        }
    }

    private IEnumerator ApplyKnockbackCoroutine(Vector2 direction, float force)
    {
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(direction * force, ForceMode2D.Impulse);
            Debug.Log($"Knockback applied with direction {direction} and force {force}");

            EnemyController controller = GetComponent<EnemyController>();
            if (controller != null)
            {
                controller.enabled = false;
            }

            yield return new WaitForSeconds(knockbackDuration);

            if (controller != null)
            {
                controller.enabled = true;
            }
        }
        else
        {
            Debug.LogError("Rigidbody2D not found on enemy for knockback");
        }
    }

    private void PlayDeathSound()
    {
        if (deathSound != null)
        {
            GameObject deathSoundObject = new GameObject("DeathSound");
            AudioSource deathAudioSource = deathSoundObject.AddComponent<AudioSource>();
            deathAudioSource.clip = deathSound;
            deathAudioSource.Play();

            Destroy(deathSoundObject, deathSound.length);
        }
    }
}