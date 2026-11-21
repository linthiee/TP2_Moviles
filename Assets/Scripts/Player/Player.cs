using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
    
    private float _jumpVelocity;
    private Rigidbody _rigidbody;

    private int _streak;
    
    private bool _hasLanded;
    public Vector3 Position => transform.position;
    public int Streak => _streak;

    private void OnEnable()
    {
        EventBus.Subscribe<OnArchPassed>(OnArchPassed);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<OnArchPassed>(OnArchPassed);
    }
    
   public void Start()
    {
        TryGetComponent(out _rigidbody);
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
            _hasLanded = true;
    }

    private void OnArchPassed(in OnArchPassed eventData)
    {
        Debug.Log("Streak! "  + _streak);
        _streak++;
    }
}