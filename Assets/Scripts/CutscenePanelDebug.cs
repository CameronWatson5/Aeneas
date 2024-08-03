using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CutscenePanelDebug : MonoBehaviour
{
    public GameObject cutscenePanel;
    public TMP_Text cutsceneText;
    public CanvasGroup canvasGroup;

    private void Awake()
    {
        if (cutscenePanel != null)
        {
            canvasGroup = cutscenePanel.GetComponent<CanvasGroup>();
            cutsceneText = cutscenePanel.GetComponentInChildren<TMP_Text>();
            Debug.Log($"CutscenePanel initialized. CanvasGroup: {canvasGroup != null}, TextComponent: {cutsceneText != null}");
        }
    }

    private void Update()
    {
        if (cutscenePanel != null)
        {
            Debug.Log($"CutscenePanel - Alpha: {canvasGroup.alpha}, Interactable: {canvasGroup.interactable}, BlocksRaycasts: {canvasGroup.blocksRaycasts}");
            Debug.Log($"CutscenePanel - Current Text: {cutsceneText.text}");
        }
    }
}