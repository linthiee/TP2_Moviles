using UnityEngine;

public static class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeGame()
    {
        ServiceProvider.Instance.AddService<EventBus>(new EventBus());
        ServiceProvider.Instance.AddService<GameSessionService>(new GameSessionService());
        
        #if UNITY_ANDROID && !UNITY_EDITOR
         ServiceProvider.Instance.AddService<IVibrationService>(new AndroidVibrationService());
        #endif
    }
}