using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
        // Remove extra spaces
        entry = Regex.Replace(entry, @"\s{2,}", " ");

        // Split the log entry if it's too long
        int maxLength = 40; 
        List<string> splitEntries = SplitLogEntry(entry, maxLength);

        foreach (string splitEntry in splitEntries)
        {
            logEntries.Add(splitEntry);
            if (logContent != null && logEntryPrefab != null)
            {
                GameObject logEntryObject = Instantiate(logEntryPrefab, logContent);
                TextMeshProUGUI textComponent = logEntryObject.GetComponent<TextMeshProUGUI>();
                if (textComponent != null)
                {
                    textComponent.text = splitEntry;
                }
            }
        }
    }

    private List<string> SplitLogEntry(string entry, int maxLength)
    {
        List<string> splitEntries = new List<string>();

        while (entry.Length > maxLength)
        {
            int lastSpaceIndex = entry.LastIndexOf(' ', maxLength);
            if (lastSpaceIndex == -1) lastSpaceIndex = maxLength;

            string splitEntry = entry.Substring(0, lastSpaceIndex).Trim();
            splitEntries.Add(splitEntry);
            entry = entry.Substring(lastSpaceIndex).Trim();
        }

        if (entry.Length > 0)
        {
            splitEntries.Add(entry);
        }

        return splitEntries;
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
