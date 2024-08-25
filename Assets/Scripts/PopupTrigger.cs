// This is used to trigger a popup.

using UnityEngine;

public class PopupTrigger : MonoBehaviour
{
    public string popupId; // Unique identifier for this popup
    public string popupText;
    public float popupDelay = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (PopUpManager.Instance != null)
            {
                PopUpManager.Instance.ShowPopupWithDelay(popupDelay, popupText, popupId);
            }
            else
            {
                Debug.LogError("PopUpManager instance not found.");
            }
        }
    }
}