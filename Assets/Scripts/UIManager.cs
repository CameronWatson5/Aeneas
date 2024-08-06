using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    public GameObject interactionPrompt;
    public TextMeshProUGUI interactionText;
    public Button pauseButton; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(PauseGame);
        }
    }

    public void ShowInteractionPrompt(string message)
    {
        if (interactionText != null)
        {
            interactionText.text = message;
        }
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(true);
        }
    }

    public void HideInteractionPrompt()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }

    public void PauseGame()
    {
        if (PauseMenu.Instance != null)
        {
            PauseMenu.Instance.Pause();
        }
        else
        {
            Debug.LogError("PauseMenu instance not found!");
        }
    }
}