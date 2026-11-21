using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private float _jumpVelocity;
    private Rigidbody _rigidbody;

    private int _streak;
    private int _bounces;

    private bool _hasLanded;

    MeshRenderer _meshRenderer;
    Collider _collider;

    public Vector3 Position => transform.position;
    public int Streak => _streak;

    public int Bounces
    {
        get => _bounces;
        set => _bounces += value;
    }

    private void OnEnable()
    {
        EventBus.Subscribe<OnArchPassed>(OnArchPassed);
        EventBus.Subscribe<OnSpikeCollision>(OnSpikeCollision);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<OnArchPassed>(OnArchPassed);
        EventBus.Unsubscribe<OnSpikeCollision>(OnSpikeCollision);
    }

    public void Start()
    {
        TryGetComponent(out _rigidbody);
        TryGetComponent(out _meshRenderer);
        TryGetComponent(out _collider);

        _jumpVelocity = 3.0f;
    }

    public void FixedUpdate()
    {
        if (_hasLanded)
        {
            _rigidbody.linearVelocity = new Vector3(_rigidbody.linearVelocity.x, _jumpVelocity, _rigidbody.linearVelocity.z);
            _hasLanded = false;
            _streak = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cyllinder"))
        {
            _hasLanded = true;
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
        _meshRenderer.enabled = false;
        _collider.enabled = false;

        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.isKinematic = true;
    }
}