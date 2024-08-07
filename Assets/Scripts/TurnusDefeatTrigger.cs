using UnityEngine;

public class TurnusDefeatTrigger : MonoBehaviour
{
    private void Start()
    {
        EnemyHealth turnusHealth = GameObject.Find("TurnusLocation").GetComponent<EnemyHealth>();
        if (turnusHealth != null)
        {
            turnusHealth.OnEnemyDeath += HandleTurnusDefeat;
        }
    }

    private void HandleTurnusDefeat(GameObject turnus)
    {
        // Mark mission target as found
        MissionManager.Instance.SetMissionTargetStatus("Turnus", true);

        // Show special popup and transition to Game Over
        if (MissionManager.Instance.IsCurrentMission(6))
        {
            MissionManager.Instance.CompleteCurrentMission();
            SpecialPopUpManager.Instance.ShowPopup("You defeated Turnus!", "GameOver");
        }
    }
}