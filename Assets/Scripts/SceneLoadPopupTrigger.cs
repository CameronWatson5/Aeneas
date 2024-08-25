// This script is used for the opening pop up which has the controls for the player.

using UnityEngine;

public class SceneLoadPopupTrigger : MonoBehaviour
{
    public string popupId;
    public string popupText;
    public float popupDelay = 0f;

    void Start()
    {
        // Trigger the popup only if the scene is in the list of scenesToShowPopup
        if (PopUpManager.Instance != null && PopUpManager.Instance.scenesToShowPopup.Contains(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name))
        {
            PopUpManager.Instance.ShowPopupOnSceneLoad(popupId, popupText, popupDelay);
        }
    }
}