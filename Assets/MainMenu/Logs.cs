using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Logs : MonoBehaviour
{
    public TextMeshProUGUI reportErrorTextObject;
    void Awake()
    {
        if (reportErrorTextObject== null)
        {
            reportErrorTextObject = GetComponent<TextMeshProUGUI>();
        }
        Application.logMessageReceived += HandleLog;
    }
 
    void HandleLog(string logString, string stackTrace, LogType type)
    {
       var output = logString;
        if (type == LogType.Exception || type == LogType.Error)
        {
            reportErrorTextObject.text = stackTrace + "\n" + output + "\n";
        }
    }
}
