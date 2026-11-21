using UnityEngine;

public class Breakable : MonoBehaviour
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
    private Player _player;

    private void Start()
    {
        _player = FindFirstObjectByType(typeof(Player)) as Player;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (CompareTag("Player") && _player.Streak >= 2)
        {
            Debug.Log("Breaking...");
            EventBus.Raise<OnArchBroken>();
            Destroy(this);
        }
    }
}