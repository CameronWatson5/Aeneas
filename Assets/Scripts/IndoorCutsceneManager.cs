// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
// using System.Collections;
// using UnityEngine.SceneManagement;
//
// public class IndoorCutsceneManager : MonoBehaviour
// {
//     public static IndoorCutsceneManager Instance { get; private set; }
//
//     public GameObject cutscenePanel;
//     public TMP_Text cutsceneText;
//     public Button skipButton;
//
//     [Header("Cutscene Settings")]
//     public float typingSpeed = 0.05f;
//     public string[] cutsceneLines;
//     public string nextSceneName;
//
//     private int currentLineIndex = 0;
//     private Image panelImage;
//     private CanvasGroup canvasGroup;
//
//     private void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject);
//             Debug.Log("IndoorCutsceneManager: Instance created and set to DontDestroyOnLoad.");
//         }
//         else
//         {
//             Destroy(gameObject);
//             Debug.Log("IndoorCutsceneManager: Duplicate instance destroyed.");
//             return;
//         }
//
//         InitializeComponents();
//     }
//
//     private void InitializeComponents()
//     {
//         if (cutscenePanel == null || cutsceneText == null || skipButton == null)
//         {
//             Debug.LogError("IndoorCutsceneManager: One or more UI components are not assigned.");
//             return;
//         }
//
//         panelImage = cutscenePanel.GetComponent<Image>();
//         canvasGroup = cutscenePanel.GetComponent<CanvasGroup>();
//
//         if (panelImage == null)
//         {
//             Debug.LogError("IndoorCutsceneManager: Image component not found on cutscenePanel.");
//         }
//
//         if (canvasGroup == null)
//         {
//             Debug.LogError("IndoorCutsceneManager: CanvasGroup component not found on cutscenePanel.");
//             return;
//         }
//
//         skipButton.onClick.AddListener(EndCutscene);
//         cutscenePanel.SetActive(false);
//     }
//
//     public void TriggerCutsceneWithDelay(float delay)
//     {
//         Debug.Log("IndoorCutsceneManager: TriggerCutsceneWithDelay called");
//         gameObject.SetActive(true);
//         StartCoroutine(DelayedCutsceneStart(delay));
//     }
//
//     private IEnumerator DelayedCutsceneStart(float delay)
//     {
//         Debug.Log($"IndoorCutsceneManager: Delaying cutscene start for {delay} seconds");
//         yield return new WaitForSeconds(delay);
//         StartCutscene();
//     }
//
//     public void StartCutscene()
//     {
//         Debug.Log("IndoorCutsceneManager: StartCutscene called");
//         if (cutscenePanel == null || cutsceneText == null)
//         {
//             Debug.LogError("IndoorCutsceneManager: cutscenePanel or cutsceneText is missing!");
//             return;
//         }
//         if (cutscenePanel.transform.parent == null)
//         {
//             Canvas mainCanvas = FindObjectOfType<Canvas>();
//             if (mainCanvas != null)
//             {
//                 cutscenePanel.transform.SetParent(mainCanvas.transform, false);
//                 Debug.Log("IndoorCutsceneManager: Cutscene panel parented to main Canvas");
//             }
//             else
//             {
//                 Debug.LogError("IndoorCutsceneManager: No Canvas found to parent cutscene panel");
//             }
//         }
//
//         cutscenePanel.SetActive(true);
//         if (canvasGroup != null)
//         {
//             canvasGroup.alpha = 1;
//             canvasGroup.interactable = true;
//             canvasGroup.blocksRaycasts = true;
//             Debug.Log("IndoorCutsceneManager: CanvasGroup settings applied");
//         }
//
//         currentLineIndex = 0;
//         StartCoroutine(PlayCutscene());
//
//         Debug.Log($"Cutscene panel active: {cutscenePanel.activeSelf}");
//         Debug.Log($"Cutscene panel rect transform - anchorMin: {cutscenePanel.GetComponent<RectTransform>().anchorMin}, " +
//                   $"anchorMax: {cutscenePanel.GetComponent<RectTransform>().anchorMax}, " +
//                   $"sizeDelta: {cutscenePanel.GetComponent<RectTransform>().sizeDelta}, " +
//                   $"anchoredPosition: {cutscenePanel.GetComponent<RectTransform>().anchoredPosition}");
//         Debug.Log($"Cutscene panel parent: {cutscenePanel.transform.parent?.name ?? "No parent"}");
//     }
//
//     private IEnumerator PlayCutscene()
//     {
//         Debug.Log("IndoorCutsceneManager: Playing cutscene");
//         while (currentLineIndex < cutsceneLines.Length)
//         {
//             yield return StartCoroutine(TypeLine(cutsceneLines[currentLineIndex]));
//             yield return new WaitForSeconds(2f);
//             currentLineIndex++;
//         }
//         EndCutscene();
//     }
//
//     private IEnumerator TypeLine(string line)
//     {
//         cutsceneText.text = "";
//         foreach (char c in line.ToCharArray())
//         {
//             cutsceneText.text += c;
//             yield return new WaitForSeconds(typingSpeed);
//         }
//         Debug.Log($"IndoorCutsceneManager: Finished typing line: {line}");
//     }
//
//     private void EndCutscene()
//     {
//         Debug.Log("IndoorCutsceneManager: Ending cutscene");
//         StopAllCoroutines();
//         cutscenePanel.SetActive(false);
//
//         if (!string.IsNullOrEmpty(nextSceneName))
//         {
//             SceneManager.LoadScene(nextSceneName);
//         }
//     }
// }
