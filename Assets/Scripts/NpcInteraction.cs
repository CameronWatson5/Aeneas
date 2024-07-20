using UnityEngine;
using TMPro;
using System.Collections;

public class NPCInteraction : MonoBehaviour
{
    public float interactionDistance = 2f;
    public string[] dialogue;
    public string npcName;
    public float typingSpeed = 0.05f; // Speed of the typewriter effect

    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    public GameObject interactionPrompt;

    public BoxCollider2D triggerCollider;

    private bool playerInRange = false;
    private int currentDialogueIndex = 0;
    private Coroutine typingCoroutine;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!dialoguePanel.activeSelf)
            {
                StartDialogue();
            }
            else
            {
                DisplayNextSentence();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            EndDialogue();
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }
        }
    }

    void StartDialogue()
    {
        if (triggerCollider != null)
        {
            triggerCollider.isTrigger = true;
        }
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
        }
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
        if (nameText != null)
        {
            nameText.text = npcName;
        }
        currentDialogueIndex = 0;
        DisplayNextSentence();
    }

    void DisplayNextSentence()
    {
        if (currentDialogueIndex >= dialogue.Length)
        {
            EndDialogue();
            return;
        }

        // Stop any ongoing typing
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        // Start typing the new sentence
        typingCoroutine = StartCoroutine(TypeSentence(dialogue[currentDialogueIndex]));
        currentDialogueIndex++;
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    void EndDialogue()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
        if (playerInRange && interactionPrompt != null)
        {
            interactionPrompt.SetActive(true);
        }
    }
}