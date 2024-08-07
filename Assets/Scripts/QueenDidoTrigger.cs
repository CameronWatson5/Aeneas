using UnityEngine;

public class QueenDidoTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Mark mission target as found
            MissionManager.Instance.SetMissionTargetStatus("QueenDido", true);

            // Show special popup and transition to Hades
            if (MissionManager.Instance.IsCurrentMission(4))
            {
                MissionManager.Instance.CompleteCurrentMission();
                SpecialPopUpManager.Instance.ShowPopup("You found Queen Dido!", "Hades");
            }
        }
    }
}