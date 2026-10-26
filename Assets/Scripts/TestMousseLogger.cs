using UnityEngine;
using UnityEngine.UI;

public class MousseLogger : MonoBehaviour
{
    [Header("UI Elements")]
    public Text textDisplayLogs;

    private AndroidJavaObject loggerManagerInstance;
    private bool isSendingLog = false;

    void Start()
    {
        gameObject.name = "MousseLoggerObj";

#if UNITY_ANDROID && !UNITY_EDITOR
        using (var loggerClass = new AndroidJavaClass("com.mousseigne.mousselogger.MousseLoggerManager"))
            loggerManagerInstance = loggerClass.CallStatic<AndroidJavaObject>("getInstance");
        
#endif
        Application.logMessageReceived += HandleUnityLog;
        RefreshLogsOnScreen();
    }

    private void HandleUnityLog(string logString, string stackTrace, LogType type)
    {
        if (isSendingLog) 
            return;
        
        isSendingLog = true;
        
        string formattedLog = $"[{type}] {logString}";
        
#if UNITY_ANDROID && !UNITY_EDITOR
        if (loggerManagerInstance != null)
            loggerManagerInstance.Call("sendLog", formattedLog);
        
#endif
        isSendingLog = false;
    }

    public void RefreshLogsOnScreen()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (loggerManagerInstance != null && textDisplayLogs != null)
        {
            string allLogs = loggerManagerInstance.Call<string>("getAllLogs");
            textDisplayLogs.text = allLogs;
        }
#else
        if (textDisplayLogs != null)
            textDisplayLogs.text = "Los logs solo funcionan en dispositivo Android";
#endif
    }

    public void RequestClearLogs()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (loggerManagerInstance != null)
            loggerManagerInstance.Call("requestClearLogs");
        
#endif
    }

    public void OnLogsCleared(string emptyMessage)
    {
        RefreshLogsOnScreen();
    }

    public void GenerateTestLog()
    {
        Debug.Log("Log de prueba generado a las: " + System.DateTime.Now);
    }
    
    void OnDestroy()
    {
        Application.logMessageReceived -= HandleUnityLog;
        loggerManagerInstance?.Dispose();
    }
}