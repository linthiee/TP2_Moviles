
public class VFXManager
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private void OnEnable()
    {
        EventBus.Subscribe<OnSpikeCollision>(OnSpikeCollision);
        EventBus.Subscribe<OnArchBroken>(OnArchBroken);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<OnSpikeCollision>(OnSpikeCollision);
        EventBus.Unsubscribe<OnArchBroken>(OnArchBroken);
    }

    private void OnArchBroken(in OnArchBroken eventData)
    {
        throw new System.NotImplementedException();
    }

    private void OnSpikeCollision(in OnSpikeCollision eventData)
    {
        throw new System.NotImplementedException();
    }
}