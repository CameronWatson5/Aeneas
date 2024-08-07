using UnityEngine;

public class ItalyVictoryTrigger : MonoBehaviour
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
        if (MissionManager.Instance != null && MissionManager.Instance.IsCurrentMission(6))
        {
            MissionManager.Instance.SetMissionTargetStatus("Turnus", true);
            SpecialPopUpManager.Instance.ShowPopup("You defeated Turnus!", "GameOver");
        }
    }
}