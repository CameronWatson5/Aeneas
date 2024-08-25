// This script is used for the chests that are in the game. Player's
// gain gold from these chests which they can spend on items at the shop.

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

        LoadChestState();

        // Get UI elements from UIManager
        UIManager uiManager = UIManager.Instance;
        dialoguePanel = uiManager.dialoguePanel;
        dialogueText = uiManager.dialogueText;
        nameText = uiManager.nameText;
        interactionPrompt = uiManager.interactionPrompt;
        interactionText = uiManager.interactionText;

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
            UIManager.Instance.ShowInteractionPrompt("Press E or tap to Interact", this);
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
        if (Input.GetKeyDown(KeyCode.E) && playerInRange && !isOpened)
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

    private void OpenChest(AeneasAttributes playerAttributes)
    {
        isOpened = true;
        spriteRenderer.sprite = openedChestSprite;
        boxCollider.enabled = false;

        PlayerPrefs.SetInt(chestID, 1);
        playerAttributes.AddGold(goldAmount);

        ShowContentsDescription();
        UIManager.Instance.HideInteractionPrompt();
    }

    private void ShowContentsDescription()
    {
        if (nameText != null && dialogueText != null && dialoguePanel != null)
        {
            nameText.text = "Chest";
            dialogueText.text = contentsDescription;
            dialoguePanel.SetActive(true);
            StartCoroutine(HideDialogueAfterDelay(displayTime));
        }
        else
        {
            Debug.LogError("Dialogue UI elements are not set.");
        }
    }

    private IEnumerator HideDialogueAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }

    private void LoadChestState()
    {
        isOpened = PlayerPrefs.GetInt(chestID, 0) == 1;
        spriteRenderer.sprite = isOpened ? openedChestSprite : closedChestSprite;
        boxCollider.enabled = !isOpened;
    }

    public void ResetChestState()
    {
        PlayerPrefs.SetInt(chestID, 0);
        LoadChestState();
    }
}