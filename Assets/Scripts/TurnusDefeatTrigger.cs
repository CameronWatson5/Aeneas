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
            SpecialPopUpManager.Instance.ShowPopup("You defeated Turnus! With Turnus fallen, the war ended. Trojan and Latin would now unite, forging a new people. From this union would rise a city like no other - Rome. Aeneas had fulfilled his destiny, laying the foundation for an empire that would shape the world for centuries to come.", "GameOver");
        }
    }
}