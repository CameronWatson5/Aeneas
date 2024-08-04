using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Compass : MonoBehaviour
{
    public Image arrowImage;
    public Transform player;
    public string troySceneName = "Troy";
    public string troySackSceneName = "TroySack"; 
    private Transform target;
    private Transform[] greekHeroes;
    private Transform[] familyMembers;
    private Transform nearestHero;
    private Transform nearestFamilyMember;

    private bool missionComplete;

    private void Start()
    {
        FindPlayer();
        FindGreekHeroes();
        FindFamilyMembers();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (player == null)
        {
            FindPlayer();
        }
        if (player != null && (IsInTroyScene() || IsInTroySackScene()))
        {
            if (missionComplete)
            {
                UpdateArrowToTarget();
            }
            else if (IsInTroySackScene())
            {
                UpdateArrowToNearestFamilyMember();
            }
            else
            {
                UpdateArrowToNearestHero();
            }
        }
        else
        {
            arrowImage.enabled = false;
        }
    }

    private void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Compass: Player not found");
        }
    }

    private void UpdateArrowToTarget()
    {
        if (target != null)
        {
            UpdateArrowDirection(target.position);
        }
        else
        {
            arrowImage.enabled = false;
        }
    }

    private void UpdateArrowToNearestFamilyMember()
    {
        FindNearestFamilyMember();
        if (nearestFamilyMember != null)
        {
            UpdateArrowDirection(nearestFamilyMember.position);
        }
        else
        {
            arrowImage.enabled = false;
        }
    }

    private void UpdateArrowToNearestHero()
    {
        FindNearestHero();
        if (nearestHero != null)
        {
            UpdateArrowDirection(nearestHero.position);
        }
        else
        {
            arrowImage.enabled = false;
        }
    }

    private void UpdateArrowDirection(Vector3 targetPosition)
    {
        if (player == null)
        {
            FindPlayer();
        }
        if (player != null)
        {
            Vector3 direction = targetPosition - player.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrowImage.rectTransform.localRotation = Quaternion.Euler(0, 0, angle - 90);
            arrowImage.enabled = true;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == troySceneName)
        {
            FindGreekHeroes();
        }
        else if (scene.name == troySackSceneName)
        {
            FindFamilyMembers();
        }
    }

    private bool IsInTroyScene()
    {
        return SceneManager.GetActiveScene().name == troySceneName;
    }

    private bool IsInTroySackScene()
    {
        return SceneManager.GetActiveScene().name == troySackSceneName;
    }

    public void FindGreekHeroes()
    {
        GameObject[] heroes = GameObject.FindGameObjectsWithTag("Boss");
        greekHeroes = new Transform[heroes.Length];
        for (int i = 0; i < heroes.Length; i++)
        {
            greekHeroes[i] = heroes[i].transform;
        }
        missionComplete = false;
    }

    public void FindFamilyMembers()
    {
        GameObject[] family = GameObject.FindGameObjectsWithTag("Family");
        familyMembers = new Transform[family.Length];
        for (int i = 0; i < family.Length; i++)
        {
            familyMembers[i] = family[i].transform;
        }
        missionComplete = false;
    }

    private void FindNearestHero()
    {
        nearestHero = FindNearestTransform(greekHeroes);
    }

    private void FindNearestFamilyMember()
    {
        nearestFamilyMember = FindNearestTransform(familyMembers);
    }

    private Transform FindNearestTransform(Transform[] targets)
    {
        float nearestDistance = float.MaxValue;
        Transform nearest = null;

        foreach (Transform target in targets)
        {
            if (target != null && target.gameObject.activeInHierarchy)
            {
                float distance = Vector3.Distance(player.position, target.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearest = target;
                }
            }
        }

        return nearest;
    }

    public void CompleteMission()
    {
        missionComplete = true;
    }

    public void SetTarget(Transform newTarget)
    {
        Debug.Log($"Compass: New target set to {newTarget.name} at position {newTarget.position}");
        target = newTarget;
        CompleteMission();
    }

}
