
using UnityEngine;

public class VibrationListener : MonoBehaviour
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private void OnEnable()
    {
        EventBus.Subscribe<OnArchBroken>(OnArchBroken);
        EventBus.Subscribe<OnSpikeCollision>(OnSpikeCollision);
        EventBus.Subscribe<OnStreakStarted>(OnStreakStarted);
    }
    
    private void OnDisable()
    {
        EventBus.Subscribe<OnArchBroken>(OnArchBroken);
        EventBus.Subscribe<OnSpikeCollision>(OnSpikeCollision);
        EventBus.Subscribe<OnStreakStarted>(OnStreakStarted);
    }

    private void OnStreakStarted(in OnStreakStarted eventData)
    {
        ServiceProvider.Instance.GetService<IVibrationService>().VibrateShort();
    }

    private void OnSpikeCollision(in OnSpikeCollision eventData)
    {
        ServiceProvider.Instance.GetService<IVibrationService>().VibrateLong();
    }

    private void OnArchBroken(in OnArchBroken eventData)
    {
        ServiceProvider.Instance.GetService<IVibrationService>().VibrateLong();
    }
}