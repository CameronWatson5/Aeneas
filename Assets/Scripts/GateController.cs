using UnityEngine;

public class GateController : MonoBehaviour, IInteractable
{
    public Sprite closedSprite;
    public Sprite openSprite;
    public string promptText = "Press E or tap Interact to open";
    public float resetTime = 3.0f;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private BoxCollider2D triggerCollider;
    private bool isOpen = false;
    private bool playerInRange = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        BoxCollider2D[] colliders = GetComponents<BoxCollider2D>();
        foreach (BoxCollider2D col in colliders)
        {
            if (col.isTrigger)
            {
                triggerCollider = col;
            }
            else
            {
                boxCollider = col;
            }
        }
    }

    void Start()
    {
        spriteRenderer.sprite = closedSprite;
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isOpen)
        {
            Interact();
        }
    }

    public void Interact()
    {
        if (!isOpen)
        {
            OpenGate();
        }
    }

    void OpenGate()
    {
        isOpen = true;
        spriteRenderer.sprite = openSprite;
        boxCollider.enabled = false;
        UIManager.Instance.HideInteractionPrompt();
        Invoke("CloseGate", resetTime);
    }

    void CloseGate()
    {
        isOpen = false;
        spriteRenderer.sprite = closedSprite;
        boxCollider.enabled = true;
        if (playerInRange)
        {
            UIManager.Instance.ShowInteractionPrompt(promptText, this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (!isOpen)
            {
                UIManager.Instance.ShowInteractionPrompt(promptText, this);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            UIManager.Instance.HideInteractionPrompt();
        }
    }
}