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
        FindGreekHeroes();
        FindFamilyMembers();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (IsInTroyScene() || IsInTroySackScene())
        {
            if (missionComplete)
            {
                if (target != null)
                {
                    Vector3 direction = target.position - player.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    arrowImage.rectTransform.localRotation = Quaternion.Euler(0, 0, angle - 90); // Adjust for arrow orientation
                    arrowImage.enabled = true;
                }
                else
                {
                    arrowImage.enabled = false;
                }
            }
            else if (IsInTroySackScene())
            {
                FindNearestFamilyMember();
                if (nearestFamilyMember != null)
                {
                    Vector3 direction = nearestFamilyMember.position - player.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    arrowImage.rectTransform.localRotation = Quaternion.Euler(0, 0, angle - 90); // Adjust for arrow orientation
                    arrowImage.enabled = true;
                }
                else
                {
                    arrowImage.enabled = false;
                }
            }
            else
            {
                FindNearestHero();
                if (nearestHero != null)
                {
                    Vector3 direction = nearestHero.position - player.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    arrowImage.rectTransform.localRotation = Quaternion.Euler(0, 0, angle - 90); // Adjust for arrow orientation
                    arrowImage.enabled = true;
                }
                else
                {
                    arrowImage.enabled = false;
                }
            }
        }
        else
        {
            arrowImage.enabled = false;
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

    private void FindGreekHeroes()
    {
        GameObject[] heroes = GameObject.FindGameObjectsWithTag("Boss");
        greekHeroes = new Transform[heroes.Length];
        for (int i = 0; i < heroes.Length; i++)
        {
            greekHeroes[i] = heroes[i].transform;
        }
    }

    private void FindFamilyMembers()
    {
        GameObject[] family = GameObject.FindGameObjectsWithTag("Family");
        familyMembers = new Transform[family.Length];
        for (int i = 0; i < family.Length; i++)
        {
            familyMembers[i] = family[i].transform;
        }
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
        target = newTarget;
        CompleteMission(); 
    }
}
