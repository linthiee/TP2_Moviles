using System;
using UnityEngine;

public class MousseLogger : MonoBehaviour, IService
{

    public bool IsPersistant => true;
    public event Action LogsUpdated;
    
    private AndroidJavaObject _loggerManagerInstance;
    private bool _isSendingLog = false;

    private void Awake()
    {
        gameObject.name = "MousseLoggerObj";
        DontDestroyOnLoad(gameObject);
        ServiceProvider.Instance.AddService<MousseLogger>(this);

#if UNITY_ANDROID && !UNITY_EDITOR
    using (var loggerClass = new AndroidJavaClass("com.mousseigne.mousselogger.MousseLoggerManager"))
        _loggerManagerInstance = loggerClass.CallStatic<AndroidJavaObject>("getInstance");
#endif
        Application.logMessageReceived += HandleUnityLog;
    }
    
    private void HandleUnityLog(string logString, string stackTrace, LogType type)
    {
        if (_isSendingLog) 
            return;
        
        _isSendingLog = true;
        
        string formattedLog = $"[{type}] {logString}";
        
#if UNITY_ANDROID && !UNITY_EDITOR
        if (_loggerManagerInstance != null)
            _loggerManagerInstance.Call("sendLog", formattedLog);
        
#endif
        _isSendingLog = false;
    }

    public string GetAllLogs()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (_loggerManagerInstance != null)
            return _loggerManagerInstance.Call<string>("getAllLogs");
        return "Logger nativo no inicializado.";
#else
        return "Los logs nativos solo funcionan en dispositivo Android.";
#endif
    }
    
    public void RequestClearLogs()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (_loggerManagerInstance != null)
            _loggerManagerInstance.Call("requestClearLogs");
#else
        OnLogsCleared(string.Empty);
#endif
    }

    public void OnLogsCleared(string emptyMessage)
    {
        LogsUpdated?.Invoke();
    }

    public void GenerateTestLog()
    {
        Debug.Log($"Log de prueba generado a las: {DateTime.Now:HH:mm:ss}");
    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= HandleUnityLog;
        _loggerManagerInstance?.Dispose();
    }
}