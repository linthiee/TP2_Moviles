using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private float _jumpVelocity;
    private Rigidbody _rigidbody;

    private int _streak;
    private int _bounces;
    private int _numToStreak;

    private bool _isOnStreak;
    private bool _hasLanded;

    MeshRenderer _meshRenderer;
    Collider _collider;

    public Vector3 Position => transform.position;
    public int Streak => _streak;
    public int NumToStreak => _numToStreak;
    
    public int Bounces
    {
        get => _bounces;
        set => _bounces = value;
    }

    private void OnEnable()
    {
        EventBus.Subscribe<OnArchPassed>(OnArchPassed);
        EventBus.Subscribe<OnSpikeCollision>(OnSpikeCollision);
        EventBus.Subscribe<OnPlayerWin>(OnPlayerWin);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<OnArchPassed>(OnArchPassed);
        EventBus.Unsubscribe<OnSpikeCollision>(OnSpikeCollision);
        EventBus.Unsubscribe<OnPlayerWin>(OnPlayerWin);
    }
    
    public void Start()
    {
        TryGetComponent(out _rigidbody);
        TryGetComponent(out _meshRenderer);
        TryGetComponent(out _collider);

        _jumpVelocity = 3.0f;
        _numToStreak = 2;
        _isOnStreak = false;
    }

    public void FixedUpdate()
    {
        if (_hasLanded)
        {
            EventBus.Raise<OnPlayerBounce>();
            
            _rigidbody.linearVelocity = new Vector3(_rigidbody.linearVelocity.x, _jumpVelocity, _rigidbody.linearVelocity.z);
            _hasLanded = false;
            _streak = 0;
        }

        if (_streak >= _numToStreak && !_isOnStreak)
        {
            Debug.Log("Streak started");
            _isOnStreak = true;
            EventBus.Raise<OnStreakStarted>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cyllinder"))
        {
            _hasLanded = true;
            _isOnStreak = false;
        }
    }

    private void OnArchPassed(in OnArchPassed eventData)
    {
        Debug.Log("Streak! " + _streak);
        _streak++;
        _bounces = 0;
    }

    private void OnSpikeCollision(in OnSpikeCollision eventData)
    {
        Debug.Log("[Player] OnSpikeCollision recibido");

        _meshRenderer.enabled = false;
        _collider.enabled = false;

        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.isKinematic = true;

        _hasLanded = false;
        _isOnStreak = false;
        _streak = 0;
        _bounces = 0;

        ServiceProvider.Instance.GetService<TaskScheduler>().Schedule(Respawm, 2.0f);
    }
    
    private void OnPlayerWin(in OnPlayerWin eventData)
    {
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.isKinematic = true;
    }

    public void Respawm()
    {
        transform.position = respawnPoint.position;

        _meshRenderer.enabled = true;
        _collider.enabled = true;

        _rigidbody.isKinematic = false;
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        EventBus.Raise<OnPlayerRespawn>();
    }
}