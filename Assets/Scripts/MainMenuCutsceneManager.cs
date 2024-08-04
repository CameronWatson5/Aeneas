using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuCutsceneManager : MonoBehaviour
{
    public TextMeshProUGUI cutsceneText;
    public float typingSpeed = 0.05f;
    public string[] cutsceneLines;
    public string nextSceneName;

    private CanvasGroup canvasGroup;
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private int currentLineIndex = 0;

    private void Awake()
    {
        Debug.Log("MainMenuCutsceneManager: Awake called");
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.Log("MainMenuCutsceneManager: CanvasGroup not found, adding one");
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 0;
    }

    private void Start()
    {
        Debug.Log("MainMenuCutsceneManager: Start called");
        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        Debug.Log("MainMenuCutsceneManager: Starting cutscene");
        canvasGroup.alpha = 1;

        if (cutsceneText == null)
        {
            Debug.LogError("MainMenuCutsceneManager: cutsceneText is null");
            yield break;
        }

        if (cutsceneLines == null || cutsceneLines.Length == 0)
        {
            Debug.LogError("MainMenuCutsceneManager: cutsceneLines is null or empty");
            yield break;
        }

        while (currentLineIndex < cutsceneLines.Length)
        {
            yield return StartCoroutine(TypeLine(cutsceneLines[currentLineIndex]));
            currentLineIndex++;
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.touchCount > 0);
            yield return new WaitForEndOfFrame(); 
        }

        EndCutscene();
    }

    private IEnumerator TypeLine(string line)
    {
        isTyping = true;
        cutsceneText.text = "";
        foreach (char c in line.ToCharArray())
        {
            cutsceneText.text += c;
            yield return new WaitForSeconds(typingSpeed);

            if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
            {
                cutsceneText.text = line;
                isTyping = false;
                yield break;
            }
        }
        isTyping = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            if (isTyping)
            {
                StopAllCoroutines();
                cutsceneText.text = cutsceneLines[currentLineIndex];
                isTyping = false;
                currentLineIndex++;
                if (currentLineIndex < cutsceneLines.Length)
                {
                    StartCoroutine(PlayCutscene());
                }
                else
                {
                    EndCutscene();
                }
            }
        }
    }

    private void EndCutscene()
    {
        Debug.Log("MainMenuCutsceneManager: Cutscene finished");
        canvasGroup.alpha = 0;

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            Debug.Log($"MainMenuCutsceneManager: Transitioning to scene: {nextSceneName}");
            // Don't destroy the cutscene panel
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.Log("MainMenuCutsceneManager: No next scene specified");
        }
    }
    public void ResetCutscene()
    {
        StopAllCoroutines();
        currentLineIndex = 0;
        isTyping = false;
        if (cutsceneText != null)
        {
            cutsceneText.text = "";
        }
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
        }
    }
}