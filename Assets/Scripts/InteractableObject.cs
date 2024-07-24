using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class InteractableObject : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionDistance = 2f;
    public string[] dialogueLines;
    public string objectName;
    public float typingSpeed = 0.05f;
    public bool useTypewriterEffect = true;

    [Header("UI Settings")]
    public string promptText = "Press E to interact";

    [Header("Debug")]
    public bool showDebugLogs = true;

    private GameObject dialoguePanel;
    private TextMeshProUGUI dialogueText;
    private TextMeshProUGUI nameText;
    private GameObject interactionPrompt;
    private TextMeshProUGUI interactionText;

    private bool playerInRange = false;
    private int currentDialogueIndex = 0;
    private Coroutine typingCoroutine;
    private BoxCollider2D triggerCollider;

    private void Awake()
    {
        triggerCollider = GetComponent<BoxCollider2D>();
        if (triggerCollider != null)
        {
            triggerCollider.isTrigger = true;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(InitializeReferences());
    }

    private void Start()
    {
        StartCoroutine(InitializeReferences());
    }

    private IEnumerator InitializeReferences()
    {
        yield return null; // Wait for a frame to ensure UI is initialized

        GameObject persistentUI = GameObject.Find("Canvas UI");
        if (persistentUI != null)
        {
            dialoguePanel = persistentUI.transform.Find("DialoguePanel")?.gameObject;
            dialogueText = dialoguePanel?.transform.Find("DialogueText")?.GetComponent<TextMeshProUGUI>();
            nameText = dialoguePanel?.transform.Find("NameText")?.GetComponent<TextMeshProUGUI>();
            interactionPrompt = persistentUI.transform.Find("InteractionPrompt")?.gameObject;
            interactionText = interactionPrompt?.transform.Find("InteractionText")?.GetComponent<TextMeshProUGUI>();

            if (dialoguePanel) dialoguePanel.SetActive(false);
            if (interactionPrompt) interactionPrompt.SetActive(false);

            if (interactionText != null)
            {
                interactionText.text = promptText;
            }

            if (showDebugLogs)
            {
                Debug.Log($"DialoguePanel found: {dialoguePanel != null}");
                Debug.Log($"DialogueText found: {dialogueText != null}");
                Debug.Log($"NameText found: {nameText != null}");
                Debug.Log($"InteractionPrompt found: {interactionPrompt != null}");
                Debug.Log($"InteractionText found: {interactionText != null}");
            }
        }
        else
        {
            Debug.LogError("Persistent UI 'Canvas UI' not found in the scene.");
        }

        playerInRange = false;
        currentDialogueIndex = 0;

        if (showDebugLogs)
        {
            Debug.Log($"References initialized for {gameObject.name}");
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (dialoguePanel != null)
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
            if (showDebugLogs)
            {
                Debug.Log($"Player entered range of {gameObject.name}");
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
            if (showDebugLogs)
            {
                Debug.Log($"Player exited range of {gameObject.name}");
            }
        }
    }

    private void StartDialogue()
    {
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
            nameText.text = objectName;
        }
        currentDialogueIndex = 0;
        DisplayNextSentence();
        if (showDebugLogs)
        {
            Debug.Log($"Started dialogue with {gameObject.name}");
        }
    }

    private void DisplayNextSentence()
    {
        if (currentDialogueIndex >= dialogueLines.Length)
        {
            EndDialogue();
            return;
        }

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        if (useTypewriterEffect && dialogueText != null)
        {
            typingCoroutine = StartCoroutine(TypeSentence(dialogueLines[currentDialogueIndex]));
        }
        else if (dialogueText != null)
        {
            dialogueText.text = dialogueLines[currentDialogueIndex];
        }
        currentDialogueIndex++;
    }

    private IEnumerator TypeSentence(string sentence)
    {
        if (dialogueText != null)
        {
            dialogueText.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
        }
    }

    private void EndDialogue()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
        if (playerInRange && interactionPrompt != null)
        {
            interactionPrompt.SetActive(true);
        }
        if (showDebugLogs)
        {
            Debug.Log($"Ended dialogue with {gameObject.name}");
        }
    }
}
