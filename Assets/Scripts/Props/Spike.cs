using System;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
    private Player _player;

    void Start()
    {
        _player = FindFirstObjectByType(typeof(Player)) as Player;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            EventBus.Raise<OnSpikeCollision>(_player.transform.position);
    }
}
