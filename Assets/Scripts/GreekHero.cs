using UnityEngine;

public class GreekHero : MonoBehaviour
{
    public string heroName; 
    public int health = 100; 

    private void Start()
    {
        if (MissionManager.Instance.missionTargets.ContainsKey(heroName) && MissionManager.Instance.missionTargets[heroName])
        {
            Destroy(gameObject); // Do not respawn if already defeated
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            OnDefeated();
            Destroy(gameObject); // Remove the hero from the game
        }
    }

    private void OnDefeated()
    {
        MissionManager.Instance.SetMissionTargetStatus(heroName, true);
    }
}