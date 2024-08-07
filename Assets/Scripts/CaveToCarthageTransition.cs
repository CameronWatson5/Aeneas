using UnityEngine;

public class CaveToCarthageTransition : MonoBehaviour
{
    public float popupDelay = 0.5f; 
    public string popupMessage;
    public string nextSceneName = "Carthage";
    public string popupId;
    public string spawnPoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("CaveToCarthageTransition: Player entered trigger");
            if (MissionManager.Instance != null)
            {
                Debug.Log($"Current mission index: {MissionManager.Instance.CurrentMissionIndex}");
                if (MissionManager.Instance.IsCurrentMission(3)) 
                {
                    Debug.Log("CaveToCarthageTransition: Correct mission, completing mission and showing special popup");
                    MissionManager.Instance.CompleteCurrentMission();
                    if (SpecialPopUpManager.Instance != null)
                    {
                        SpecialPopUpManager.Instance.ShowPopupWithDelay(popupDelay, popupMessage, nextSceneName, popupId, spawnPoint);
                    }
                    else
                    {
                        Debug.LogError("CaveToCarthageTransition: SpecialPopUpManager.Instance is null");
                    }
                }
                else
                {
                    Debug.Log("CaveToCarthageTransition: Not the correct mission");
                }
            }
            else
            {
                Debug.LogError("CaveToCarthageTransition: MissionManager.Instance is null");
            }
        }
    }
}