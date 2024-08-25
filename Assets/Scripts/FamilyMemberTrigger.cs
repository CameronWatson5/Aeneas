// This script is used in mission 3 for when the player finds their family members.

using UnityEngine;

public class FamilyMemberTrigger : MonoBehaviour
{
    public string familyMemberName;
    public string popupMessage;

    private void Start()
    {
        // Check if this family member has already been found (status saved in MissionManager)
        if (MissionManager.Instance.missionTargets.ContainsKey(familyMemberName) && MissionManager.Instance.missionTargets[familyMemberName])
        {
            // If already found, destroy this family member trigger game object
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Player found {familyMemberName}");

            // Mark this family member as found in the MissionManager
            if (MissionManager.Instance != null)
            {
                MissionManager.Instance.SetMissionTargetStatus(familyMemberName, true);
            }

            // Trigger the popup
            if (PopUpManager.Instance != null)
            {
                PopUpManager.Instance.ShowPopup(popupMessage, familyMemberName);
            }

            // Disable the game object to avoid multiple triggers
            gameObject.SetActive(false);
        }
    }
}