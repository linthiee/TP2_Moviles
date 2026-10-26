using UnityEngine;

public class TestMousseLogger : MonoBehaviour
{
#if UNITY_ANDROID

    AndroidJavaObject loggerManagerInstance;

    void Start()
    {
        using var loggerClass = new AndroidJavaClass("com.mousseigne.mousselogger.MousseLoggerManager");
        loggerManagerInstance = loggerClass.CallStatic<AndroidJavaObject>("getInstance");

        loggerManagerInstance.Call("sendLog", "Hola desde unity");
        long time = loggerManagerInstance.Call<long>("getTime");
        Debug.Log($"Hora nativa: {time}");
    }

    void OnDestroy() => loggerManagerInstance?.Dispose();

#endif
}
