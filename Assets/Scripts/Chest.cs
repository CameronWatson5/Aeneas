using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Chest : MonoBehaviour, IInteractable
{
    public Sprite closedChestSprite;
    public Sprite openedChestSprite;
    public string contentsDescription = "5 gold coins";
    public int goldAmount = 5;
    public string chestID;  // Unique ID for the chest
    public float displayTime = 3f; // Timer variable to be adjusted in the Inspector

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
            UIManager.Instance.ShowInteractionPrompt("Press E or tap Interact to open chest", this);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            UIManager.Instance.HideInteractionPrompt();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange)
        {
            Interact();
        }
    }

    public void Interact()
    {
        if (!isOpened)
        {
            OpenChest(player.GetComponent<AeneasAttributes>());
        }
    }

    private void ShowInteractionPrompt()
    {
        if (interactionText != null && interactionPrompt != null)
        {
            interactionText.text = "Press E or tap Interact to open chest";
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
    }

    private void ShowContentsDescription()
    {
        if (nameText != null && dialogueText != null && dialoguePanel != null)
        {
            nameText.text = "Chest";
            dialogueText.text = contentsDescription;
            dialoguePanel.SetActive(true);
            isDisplayingContents = true;
            StartCoroutine(HideDialogueAfterDelay(displayTime)); // Start coroutine to hide dialogue after the set time
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

    private IEnumerator HideDialogueAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideContentsDescription();
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
