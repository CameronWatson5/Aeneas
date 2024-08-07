using UnityEngine;

public class CarthageToHadesTransition : MonoBehaviour
{
    public float popupDelay = 0.5f; 
    public string popupMessage = "Transitioning to Hades.";
    public string nextSceneName = "Hades";
    public string popupId;
    public string spawnPoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("CarthageToHadesTransition: Player entered trigger");
            if (MissionManager.Instance != null)
            {
                Debug.Log($"Current mission index: {MissionManager.Instance.CurrentMissionIndex}");
                if (MissionManager.Instance.IsCurrentMission(4)) 
                {
                    Debug.Log("CarthageToHadesTransition: Correct mission, completing mission and showing special popup");
                    MissionManager.Instance.CompleteCurrentMission();
                    if (SpecialPopUpManager.Instance != null)
                    {
                        SpecialPopUpManager.Instance.ShowPopupWithDelay(popupDelay, popupMessage, nextSceneName, popupId, spawnPoint);
                    }
                    else
                    {
                        Debug.LogError("CarthageToHadesTransition: SpecialPopUpManager.Instance is null");
                    }
                }
                else
                {
                    Debug.Log("CarthageToHadesTransition: Not the correct mission");
                }
            }
            else
            {
                Debug.LogError("CarthageToHadesTransition: MissionManager.Instance is null");
            }
        }
    }
}