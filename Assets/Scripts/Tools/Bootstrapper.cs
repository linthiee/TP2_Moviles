using UnityEngine;

public static class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeGame()
    {
        ServiceProvider.Instance.AddService<EventBus>(new EventBus());
        ServiceProvider.Instance.AddService<GameSessionService>(new GameSessionService());
        ServiceProvider.Instance.AddService<CurrencyService>(new CurrencyService());
        ServiceProvider.Instance.AddService<ShopService>(new ShopService());
        ServiceProvider.Instance.AddService<GPGSManager>(new GPGSManager());
        
        #if UNITY_ANDROID && !UNITY_EDITOR
         ServiceProvider.Instance.AddService<IVibrationService>(new AndroidVibrationService());
        #endif
    }
}