using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SpecialPopUpManager : MonoBehaviour
{
    public static SpecialPopUpManager Instance { get; private set; }

    public GameObject specialPopUpPrefab;
    private SpecialPopUp specialPopUp;

    public List<string> scenesToShowPopup;
    private HashSet<string> shownPopups; // To track shown popups
    private string nextSceneName;
    private bool isGameOverPopup = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            Debug.Log($"SpecialPopUpManager Awake. specialPopUpPrefab: {(specialPopUpPrefab != null ? "Assigned" : "Not Assigned")}");
            shownPopups = new HashSet<string>(); // Initialize the set
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetupSpecialPopUp(scene.name);
    }

    private void SetupSpecialPopUp(string sceneName)
    {
        if (specialPopUp == null && specialPopUpPrefab != null && scenesToShowPopup.Contains(sceneName))
        {
            Canvas canvas = FindUICanvas();
            if (canvas != null)
            {
                Debug.Log($"Canvas found: {canvas.name}");
                GameObject specialPopUpInstance = Instantiate(specialPopUpPrefab, canvas.transform);
                specialPopUp = specialPopUpInstance.GetComponent<SpecialPopUp>();

                if (specialPopUp == null)
                {
                    Debug.LogError("SpecialPopUp component is not found on the instantiated prefab.");
                }
                else
                {
                    Debug.Log("SpecialPopUp component successfully instantiated.");
                    RectTransform rectTransform = specialPopUp.GetComponent<RectTransform>();
                    if (rectTransform != null)
                    {
                        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                        rectTransform.anchoredPosition = Vector2.zero;

                        rectTransform.localPosition = Vector3.zero;
                        rectTransform.localScale = Vector3.one;
                    }

                    Debug.Log($"Popup panel active state: {specialPopUp.gameObject.activeSelf}");

                    specialPopUp.gameObject.SetActive(false);

                    // Subscribe to the popup closed event
                    specialPopUp.OnPopupClosed += HandlePopupClosed;
                }
            }
            else
            {
                Debug.LogError("No UI Canvas found in the scene to parent the SpecialPopUp prefab.");
            }
        }
    }

    private Canvas FindUICanvas()
    {
        Canvas[] canvases = FindObjectsOfType<Canvas>();

        foreach (var canvas in canvases)
        {
            if (canvas.CompareTag("UICanvas"))
            {
                return canvas;
            }
        }

        foreach (var canvas in canvases)
        {
            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                return canvas;
            }
        }

        return null;
    }

    public void ShowPopup(string text, string nextScene)
    {
        if (specialPopUp != null)
        {
            specialPopUp.ShowPopup(text);
            Debug.Log($"Popup panel active state after ShowPopup: {specialPopUp.gameObject.activeSelf}");
            nextSceneName = nextScene;
            isGameOverPopup = nextScene == "GameOver";
        }
        else
        {
            Debug.LogError("SpecialPopUp component is not assigned in SpecialPopUpManager.");
        }
    }

    public void ShowPopupWithDelay(float delay, string text, string nextScene, string popupId)
    {
        if (shownPopups.Contains(popupId))
        {
            Debug.Log($"Popup with ID {popupId} has already been shown.");
            return;
        }

        if (specialPopUp != null)
        {
            StartCoroutine(ShowPopupAfterDelay(delay, text, nextScene, popupId));
            Debug.Log($"Popup panel active state after ShowPopupWithDelay: {specialPopUp.gameObject.activeSelf}");
        }
        else
        {
            Debug.LogError("SpecialPopUp component is not assigned in SpecialPopUpManager.");
        }
    }

    private IEnumerator ShowPopupAfterDelay(float delay, string text, string nextScene, string popupId)
    {
        yield return new WaitForSecondsRealtime(delay);
        ShowPopup(text, nextScene);
        shownPopups.Add(popupId); // Mark popup as shown
    }

    private void HandlePopupClosed()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            if (isGameOverPopup)
            {
                SceneManager.LoadScene("GameOver");
            }
            else
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}
