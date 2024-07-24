using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    [SerializeField] private float fadeDuration = 1f;
    private CanvasGroup fadeCanvasGroup;
    private string targetSceneName;
    private string spawnPointIdentifier;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupFadeCanvas();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SetupFadeCanvas()
    {
        GameObject fadeCanvas = new GameObject("Fade Canvas");
        fadeCanvas.transform.SetParent(transform);
        Canvas canvas = fadeCanvas.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;

        CanvasScaler scaler = fadeCanvas.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        fadeCanvasGroup = fadeCanvas.AddComponent<CanvasGroup>();

        GameObject fadeImage = new GameObject("Fade Image");
        fadeImage.transform.SetParent(fadeCanvas.transform);
        RectTransform rectTransform = fadeImage.AddComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;
        rectTransform.anchoredPosition = Vector2.zero;

        Image image = fadeImage.AddComponent<Image>();
        image.color = Color.black;

        fadeCanvasGroup.alpha = 0;
    }

    public void TransitionToScene(string sceneName, string spawnPoint)
    {
        targetSceneName = sceneName;
        spawnPointIdentifier = spawnPoint;
        StartCoroutine(FadeAndLoadScene());
    }

    private IEnumerator FadeAndLoadScene()
    {
        yield return StartCoroutine(Fade(1f));
        SceneManager.LoadScene(targetSceneName);
        PlayerPrefs.SetString("SpawnPointIdentifier", spawnPointIdentifier);
        yield return StartCoroutine(Fade(0f));
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeCanvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = targetAlpha;
    }

    public void ResetChestStates()
    {
        PlayerPrefs.DeleteAll(); 
    }

    public void StartNewGame(string sceneName, string spawnPoint)
    {
        ResetChestStates();
        TransitionToScene(sceneName, spawnPoint);
    }
}
