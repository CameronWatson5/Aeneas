// This script is used at the end of mission 6.
// It transitiions the player from Hades to Italy, after the player finds their father in Hades.

using UnityEngine;

public class HadesToItalyTransition : MonoBehaviour
{
    public float popupDelay = 0.5f; 
    public string popupMessage;
    public string nextSceneName = "Italy";
    public string popupId;
    public string spawnPoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("HadesToItalyTransition: Player entered trigger");
            if (MissionManager.Instance != null)
            {
                Debug.Log($"Current mission index: {MissionManager.Instance.CurrentMissionIndex}");
                if (MissionManager.Instance.IsCurrentMission(5)) 
                {
                    Debug.Log("HadesToItalyTransition: Correct mission, completing mission and showing special popup");
                    MissionManager.Instance.CompleteCurrentMission();
                    if (SpecialPopUpManager.Instance != null)
                    {
                        SpecialPopUpManager.Instance.ShowPopupWithDelay(popupDelay, popupMessage, nextSceneName, popupId, spawnPoint);
                    }
                    else
                    {
                        Debug.LogError("HadesToItalyTransition: SpecialPopUpManager.Instance is null");
                    }
                }
                else
                {
                    Debug.Log("HadesToItalyTransition: Not the correct mission");
                }
            }
            else
            {
                Debug.LogError("HadesToItalyTransition: MissionManager.Instance is null");
            }
        }
    }
}