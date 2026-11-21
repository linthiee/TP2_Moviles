using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private float _jumpVelocity;
    private Rigidbody _rigidbody;

    private bool _hasLanded;

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
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cyllinder"))
            _hasLanded = true;
    }

}