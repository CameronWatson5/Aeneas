using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PopUpManager : MonoBehaviour
{
    public static PopUpManager Instance { get; private set; }

    public GameObject popUpPrefab;
    private PopUp popUp;

    public List<string> scenesToShowPopup;
    private HashSet<string> shownPopups; // To track shown popups

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            Debug.Log($"PopUpManager Awake. popUpPrefab: {(popUpPrefab != null ? "Assigned" : "Not Assigned")}");
            shownPopups = new HashSet<string>(); // Initialize the set
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetupPopUp(scene.name);
    }

    private void SetupPopUp(string sceneName)
    {
        if (popUp == null && popUpPrefab != null)
        {
            Canvas canvas = FindUICanvas();
            if (canvas != null)
            {
                Debug.Log($"Canvas found: {canvas.name}");
                GameObject popUpInstance = Instantiate(popUpPrefab, canvas.transform);
                popUp = popUpInstance.GetComponent<PopUp>();

                if (popUp == null)
                {
                    Debug.LogError("PopUp component is not found on the instantiated prefab.");
                }
                else
                {
                    Debug.Log("PopUp component successfully instantiated.");
                    RectTransform rectTransform = popUp.GetComponent<RectTransform>();
                    if (rectTransform != null)
                    {
                        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                        rectTransform.anchoredPosition = Vector2.zero;

                        rectTransform.localPosition = Vector3.zero;
                        rectTransform.localScale = Vector3.one;
                    }

                    Debug.Log($"Popup panel active state: {popUp.gameObject.activeSelf}");

                    // Remove the default popup show logic from here
                    popUp.gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.LogError("No UI Canvas found in the scene to parent the PopUp prefab.");
            }
        }
    }

    private Canvas FindUICanvas()
    {
        Canvas[] canvases = FindObjectsOfType<Canvas>();

        foreach (var canvas in canvases)
        {
            if (canvas.CompareTag("UICanvas") && canvas.name != "Fade Canvas")
            {
                return canvas;
            }
        }

        foreach (var canvas in canvases)
        {
            if (canvas.name == "Canvas UI" && canvas.name != "Fade Canvas")
            {
                return canvas;
            }
        }

        foreach (var canvas in canvases)
        {
            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay && canvas.name != "Fade Canvas")
            {
                return canvas;
            }
        }

        return null;
    }

    public void ShowPopup(string text, string popupId)
    {
        if (shownPopups.Contains(popupId))
        {
            Debug.Log($"Popup with ID {popupId} has already been shown.");
            return;
        }

        if (popUp != null)
        {
            popUp.ShowPopup(text, popupId);
            shownPopups.Add(popupId); // Mark popup as shown
            Debug.Log($"Popup panel active state after ShowPopup: {popUp.gameObject.activeSelf}");
            LogManager.Instance?.AddLogEntry(text); // Add log entry
        }
        else
        {
            Debug.LogError("PopUp component is not assigned in PopUpManager.");
        }
    }

    public void ShowPopupOnSceneLoad(string popupId, string popupText, float popupDelay)
    {
        if (shownPopups.Contains(popupId))
        {
            Debug.Log($"Popup with ID {popupId} has already been shown.");
            return;
        }

        if (popUp != null)
        {
            popUp.gameObject.SetActive(true);
            popUp.ShowPopupOnSceneLoad(popupId, popupText, popupDelay);
            shownPopups.Add(popupId); // Mark popup as shown
            Debug.Log($"Popup panel active state after ShowPopupOnSceneLoad: {popUp.gameObject.activeSelf}");
            LogManager.Instance?.AddLogEntry(popupText); // Add log entry
        }
        else
        {
            Debug.LogError("PopUp component is not assigned in PopUpManager.");
        }
    }

    public void ShowPopupWithDelay(float delay, string text, string popupId)
    {
        if (shownPopups.Contains(popupId))
        {
            Debug.Log($"Popup with ID {popupId} has already been shown.");
            return;
        }

        if (popUp != null)
        {
            StartCoroutine(popUp.ShowPopupWithDelay(delay, text, popupId));
            shownPopups.Add(popupId); // Mark popup as shown
            Debug.Log($"Popup panel active state after ShowPopupWithDelay: {popUp.gameObject.activeSelf}");
            LogManager.Instance?.AddLogEntry(text); // Add log entry
        }
        else
        {
            Debug.LogError("PopUp component is not assigned in PopUpManager.");
        }
    }
}
