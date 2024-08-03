using UnityEngine;

public class FamilyMemberTrigger : MonoBehaviour
{
    public string familyMemberName;
    public string popupMessage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Player found {familyMemberName}");
            if (MissionManager.Instance != null)
            {
                MissionManager.Instance.SetMissionTargetStatus(familyMemberName, true);
            }

            // Trigger the popup
            if (PopUpManager.Instance != null)
            {
                PopUpManager.Instance.ShowPopup(popupMessage, familyMemberName);
            }

            // Disable the game object
            gameObject.SetActive(false);
        }
    }
}