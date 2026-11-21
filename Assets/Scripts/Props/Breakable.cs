using UnityEngine;

public class Breakable : MonoBehaviour
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
    
    private Player _player;

    private MeshRenderer _mesh;
    private MeshCollider _collider;
    
    private void Start()
    {
        TryGetComponent(out _mesh);
        TryGetComponent(out _collider);
        
        _player = FindFirstObjectByType(typeof(Player)) as Player;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && _player.Streak >= 2)
        {
            Debug.Log("Breaking...");
            EventBus.Raise<OnArchBroken>(_player.Position);
        
            _mesh.enabled = false;
            _collider.enabled = false;
        }
        else
        {
            Debug.Log("Aa");
        }
    }
}