using UnityEngine;

public class AndroidVibrationService : IVibrationService
{
    public bool IsEnabled { get; set; }

#if UNITY_ANDROID && !UNITY_EDITOR
    private readonly AndroidJavaObject _vibrator;
#endif

    public AndroidVibrationService()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        using (var activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        using (var currentActivity = activityClass.GetStatic<AndroidJavaObject>("currentActivity"))
        {
            _vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
        }
        
        IsEnabled = true;
#endif
    }

    public void VibrateShort()
    {
        Vibrate(25, 80);
    }

    public void VibrateLong()
    {
        Vibrate(75, 255);
    }

    private void Vibrate(long milliseconds, int amplitude)
    {
        if (!IsEnabled)
            return;
#if UNITY_ANDROID && !UNITY_EDITOR
        using (var vibrationEffect = new AndroidJavaClass("android.os.VibrationEffect"))
        using (var effect = vibrationEffect.CallStatic<AndroidJavaObject>("createOneShot", milliseconds, amplitude))
        {
                _vibrator.Call("vibrate", effect);
        }
#endif
    }

    public bool IsPersistant => false;
}