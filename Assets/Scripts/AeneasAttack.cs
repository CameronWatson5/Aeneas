using UnityEngine;
using System.Collections;

public class AeneasAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private float attackOffset = 0.5f; // Distance from player center to attack center
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float knockbackForce = 10f; // Increased knockback force

    [Header("Character Center Adjustment")]
    [SerializeField] private Vector2 characterCenterOffset = Vector2.zero; // Adjust this in the inspector

    [Header("Visual Settings")]
    [SerializeField] private float attackVisualDuration = 0.5f;
    [SerializeField] private Color attackVisualColor = new Color(1, 0, 0, 0.5f);

    private float lastAttackTime;
    private Animator animator;
    private PlayerMovement playerMovement;
    private GameObject attackRangeVisual;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();

        if (!animator || !playerMovement)
        {
            Debug.LogError("Required components missing on the player!");
            enabled = false;
            return;
        }

        CreateAttackRangeVisual();
        Debug.Log("AeneasAttack initialized.");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && CanAttack())
        {
            Attack();
        }
    }

    private bool CanAttack()
    {
        return Time.time >= lastAttackTime + attackCooldown;
    }

    private void Attack()
    {
        lastAttackTime = Time.time;
        Vector2 attackDirection = DetermineAttackDirection();
        string attackTrigger = GetAttackTrigger(attackDirection);
        animator.SetTrigger(attackTrigger);

        PerformAttack(attackDirection);
        StartCoroutine(ShowAttackVisual(attackDirection));

        Debug.Log($"Attack performed: Direction={attackTrigger}, Radius={attackRadius}");
    }

    private Vector2 DetermineAttackDirection()
    {
        Vector2 lastDirection = playerMovement.GetLastMovementDirection();
        
        if (Mathf.Abs(lastDirection.x) > Mathf.Abs(lastDirection.y))
        {
            return lastDirection.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            return lastDirection.y > 0 ? Vector2.up : Vector2.down;
        }
    }

    private string GetAttackTrigger(Vector2 direction)
    {
        if (direction == Vector2.right) return "AttackRight";
        if (direction == Vector2.left) return "AttackLeft";
        if (direction == Vector2.up) return "AttackUp";
        return "AttackDown";
    }

    private Vector2 GetCharacterCenter()
    {
        return (Vector2)transform.position + characterCenterOffset;
    }

    private void PerformAttack(Vector2 attackDirection)
    {
        Vector2 attackCenter = GetCharacterCenter() + attackDirection * attackOffset;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackCenter, attackRadius);

        foreach (Collider2D collider in hitColliders)
        {
            if (collider.TryGetComponent(out EnemyHealth enemy))
            {
                Debug.Log($"Damage dealt to {collider.name} with knockback direction {attackDirection} and force {knockbackForce}");
                enemy.TakeDamage(attackDamage, attackDirection, knockbackForce);
            }
        }

        Debug.Log($"Enemies hit: {hitColliders.Length}");
    }

    private IEnumerator ShowAttackVisual(Vector2 attackDirection)
    {
        UpdateAttackRangeVisual(attackDirection);
        attackRangeVisual.SetActive(true);

        yield return new WaitForSeconds(attackVisualDuration);

        attackRangeVisual.SetActive(false);
    }

    private void CreateAttackRangeVisual()
    {
        attackRangeVisual = new GameObject("AttackRangeVisual");
        attackRangeVisual.transform.SetParent(transform);

        SpriteRenderer spriteRenderer = attackRangeVisual.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = CreateCircleSprite();
        spriteRenderer.color = attackVisualColor;
        spriteRenderer.sortingLayerName = "Aeneas";
        spriteRenderer.sortingOrder = 1;

        attackRangeVisual.SetActive(false);
    }

    private Sprite CreateCircleSprite()
    {
        int textureSize = 128;
        Texture2D texture = new Texture2D(textureSize, textureSize);
        Color[] colors = new Color[textureSize * textureSize];

        for (int y = 0; y < textureSize; y++)
        {
            for (int x = 0; x < textureSize; x++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), new Vector2(textureSize / 2, textureSize / 2));
                colors[y * textureSize + x] = distance < textureSize / 2 ? Color.white : Color.clear;
            }
        }

        texture.SetPixels(colors);
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, textureSize, textureSize), new Vector2(0.5f, 0.5f));
    }

    private void UpdateAttackRangeVisual(Vector2 attackDirection)
    {
        if (attackRangeVisual != null)
        {
            Vector2 visualPosition = GetCharacterCenter() + attackDirection * attackOffset;
            attackRangeVisual.transform.position = visualPosition;
            attackRangeVisual.transform.localScale = Vector3.one * (attackRadius * 2);

            Debug.Log($"Attack visual: Position={visualPosition}, Scale={attackRangeVisual.transform.localScale}");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 characterCenter = GetCharacterCenter();
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(characterCenter, 0.1f); // Draw character center

        if (playerMovement != null)
        {
            Vector2 attackDirection = DetermineAttackDirection();
            Vector2 attackCenter = characterCenter + attackDirection * attackOffset;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackCenter, attackRadius);
        }
    }
}
