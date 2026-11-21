using UnityEngine;

public class Breakable : MonoBehaviour
{
    private int _maxLife;
    private int _life;
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private Player _player;
    private MeshRenderer _mesh;
    private MeshCollider _collider;

    private void Awake()
    {
        TryGetComponent(out _mesh);
        TryGetComponent(out _collider);
    }

    private void Start()
    {
        _player = FindFirstObjectByType<Player>();
        ResetState();
    }

    public void ResetState()
    {
        _maxLife = 3;
        _life = _maxLife;
        _mesh.enabled = true;
        _collider.enabled = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;

        if (_player.Streak >= _player.NumToStreak)
        {
            Break();
            return;
        }

        _life -= _player.Bounces;
        if (_life <= 0)
            Break();

        _player.Bounces++;
    }

    private void Break()
    {
        EventBus.Raise<OnArchBroken>(_player.Position);

        _mesh.enabled = false;
        _collider.enabled = false;
    }
}