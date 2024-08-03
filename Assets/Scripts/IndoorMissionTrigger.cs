using UnityEngine;

public class IndoorMissionTrigger : MonoBehaviour
{
    public float popupDelay = 0.5f; 
    public string popupMessage;
    public string nextSceneName;
    public string popupId;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("IndoorMissionTrigger: Player entered trigger");
            if (MissionManager.Instance != null)
            {
                Debug.Log($"Current mission index: {MissionManager.Instance.CurrentMissionIndex}");
                if (MissionManager.Instance.IsCurrentMission(1)) 
                {
                    Debug.Log("IndoorMissionTrigger: Correct mission, completing mission and showing special popup");
                    MissionManager.Instance.CompleteCurrentMission(); // Explicitly complete the mission
                    if (SpecialPopUpManager.Instance != null)
                    {
                        SpecialPopUpManager.Instance.ShowPopupWithDelay(popupDelay, popupMessage, nextSceneName, popupId);
                    }
                    else
                    {
                        Debug.LogError("IndoorMissionTrigger: SpecialPopUpManager.Instance is null");
                    }
                }
                else
                {
                    Debug.Log("IndoorMissionTrigger: Not the correct mission");
                }
            }
            else
            {
                Debug.LogError("IndoorMissionTrigger: MissionManager.Instance is null");
            }
        }
    }
}