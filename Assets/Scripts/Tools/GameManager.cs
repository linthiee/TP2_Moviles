
using UnityEngine;

public class GameManager
{
    [SerializeField] private GameObject player;
    
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private void OnEnable()
    {
        EventBus.Subscribe<OnSpikeCollision>(OnSpikeCollision);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<OnSpikeCollision>(OnSpikeCollision);
    }
    
    private void OnSpikeCollision(in OnSpikeCollision eventData)
    {
        throw new System.NotImplementedException();
    }
}