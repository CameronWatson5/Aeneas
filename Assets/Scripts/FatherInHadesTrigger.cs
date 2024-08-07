using UnityEngine;

public class FatherInHadesTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Mark mission target as found
            MissionManager.Instance.SetMissionTargetStatus("FatherInHades", true);

            // Show special popup and transition to Italy
            if (MissionManager.Instance.IsCurrentMission(5))
            {
                MissionManager.Instance.CompleteCurrentMission();
                SpecialPopUpManager.Instance.ShowPopup("You found your father in Hades!", "Italy");
            }
        }
    }
}