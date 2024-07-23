using UnityEngine;
using TMPro;

public class GateController : MonoBehaviour
{
    public Sprite closedSprite;
    public Sprite openSprite;
    public TextMeshProUGUI interactionText;
    public GameObject interactionPromptCanvas;
    public string promptText = "Press E to open the gate";
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
        interactionPromptCanvas.SetActive(false);
    }

    void Start()
    {
        spriteRenderer.sprite = closedSprite;
        interactionText.text = promptText;
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isOpen)
        {
            OpenGate();
        }
    }

    void OpenGate()
    {
        isOpen = true;
        spriteRenderer.sprite = openSprite;
        boxCollider.enabled = false;
        interactionPromptCanvas.SetActive(false);
        Invoke("CloseGate", resetTime);
    }

    void CloseGate()
    {
        isOpen = false;
        spriteRenderer.sprite = closedSprite;
        boxCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            interactionPromptCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactionPromptCanvas.SetActive(false);
        }
    }
}
