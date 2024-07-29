using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class LogManager : MonoBehaviour
{
    public static LogManager Instance { get; private set; }

    private Transform logContent;
    private GameObject logEntryPrefab;

    private List<string> logEntries = new List<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetLogContent(Transform content)
    {
        logContent = content;
        InitializeLog();
    }

    public void SetLogEntryPrefab(GameObject prefab)
    {
        logEntryPrefab = prefab;
        InitializeLog();
    }

    public void AddLogEntry(string entry)
    {
        logEntries.Add(entry);
        if (logContent != null && logEntryPrefab != null)
        {
            GameObject logEntryObject = Instantiate(logEntryPrefab, logContent);
            TextMeshProUGUI textComponent = logEntryObject.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = entry;
            }
        }
    }

    private void InitializeLog()
    {
        if (logContent != null && logEntryPrefab != null)
        {
            foreach (Transform child in logContent)
            {
                Destroy(child.gameObject);
            }

            foreach (string entry in logEntries)
            {
                GameObject logEntryObject = Instantiate(logEntryPrefab, logContent);
                TextMeshProUGUI textComponent = logEntryObject.GetComponent<TextMeshProUGUI>();
                if (textComponent != null)
                {
                    textComponent.text = entry;
                }
            }
        }
    }
}
