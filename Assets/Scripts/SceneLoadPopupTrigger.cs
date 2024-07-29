using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadPopupTrigger : MonoBehaviour
{
    public string popupId; // Unique identifier for this popup
    public string popupText;
    public float popupDelay = 1f;

    void Start()
    {
        PopUp popup = FindObjectOfType<PopUp>();
        if (popup != null)
        {
            popup.ShowPopupOnSceneLoad(popupId, popupText, popupDelay);
        }
        else
        {
            Debug.LogError("PopUp instance not found in the scene.");
        }
    }
}