using UnityEngine;
using TMPro;

public class Chest : MonoBehaviour
{
    public Sprite closedChestSprite;
    public Sprite openedChestSprite;
    public string contentsDescription = "5 gold coins";
    public int goldAmount = 5;
    public string chestID;  // Unique ID for the chest

    private bool isOpened = false;
    private bool isDisplayingContents = false;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private GameObject player;
    private bool playerInRange = false;

    private GameObject dialoguePanel;
    private TextMeshProUGUI dialogueText;
    private TextMeshProUGUI nameText;
    private GameObject interactionPrompt;
    private TextMeshProUGUI interactionText;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        // Load the state of the chest
        LoadChestState();

        // Get UI elements from UIManager
        dialoguePanel = UIManager.Instance.dialoguePanel;
        dialogueText = UIManager.Instance.dialogueText;
        nameText = UIManager.Instance.nameText;
        interactionPrompt = UIManager.Instance.interactionPrompt;
        interactionText = UIManager.Instance.interactionText;

        if (dialoguePanel == null || dialogueText == null || nameText == null || interactionPrompt == null || interactionText == null)
        {
            Debug.LogError("One or more UI elements are not found or not assigned properly.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isOpened)
        {
            playerInRange = true;
            player = other.gameObject;
            ShowInteractionPrompt();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            HideInteractionPrompt();
            if (!isOpened)
            {
                HideContentsDescription();
                EnablePlayerMovement();
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (playerInRange && !isOpened)
            {
                OpenChest(player.GetComponent<AeneasAttributes>());
            }
            else if (isDisplayingContents)
            {
                HideContentsDescription();
                EnablePlayerMovement();
            }
        }
    }

    private void ShowInteractionPrompt()
    {
        if (interactionText != null && interactionPrompt != null)
        {
            interactionText.text = "Press E to interact";  // Set common text for interaction
            interactionPrompt.SetActive(true);
        }
        else
        {
            Debug.LogError("Interaction prompt or text is not set.");
        }
    }

    private void HideInteractionPrompt()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }

    private void OpenChest(AeneasAttributes playerAttributes)
    {
        isOpened = true;
        spriteRenderer.sprite = openedChestSprite;
        boxCollider.enabled = false;

        // Save the state of the chest
        PlayerPrefs.SetInt(chestID, 1);

        // Update player's attributes
        playerAttributes.AddGold(goldAmount);

        // Show the contents description in the dialogue panel
        ShowContentsDescription();
        HideInteractionPrompt();
        DisablePlayerMovement();
    }

    private void ShowContentsDescription()
    {
        if (nameText != null && dialogueText != null && dialoguePanel != null)
        {
            nameText.text = "Chest";
            dialogueText.text = contentsDescription;
            dialoguePanel.SetActive(true);
            isDisplayingContents = true;
        }
        else
        {
            Debug.LogError("Dialogue UI elements are not set.");
        }
    }

    private void HideContentsDescription()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
            isDisplayingContents = false;
        }
    }

    private void DisablePlayerMovement()
    {
        if (player != null)
        {
            var playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.CanMove = false;
            }
        }
    }

    private void EnablePlayerMovement()
    {
        if (player != null)
        {
            var playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.CanMove = true;
            }
        }
    }

    private void LoadChestState()
    {
        if (PlayerPrefs.GetInt(chestID, 0) == 1)
        {
            isOpened = true;
            spriteRenderer.sprite = openedChestSprite;
            boxCollider.enabled = false;
        }
        else
        {
            isOpened = false;
            spriteRenderer.sprite = closedChestSprite;
            boxCollider.enabled = true;
        }
    }

    public void ResetChestState()
    {
        PlayerPrefs.SetInt(chestID, 0);
        LoadChestState();
    }
}
