using System;
using UnityEngine;

public class PooledVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    private Action<PooledVFX> onFinished;

    public void Initialize(Action<PooledVFX> returnToPoolCallback)
    {
        onFinished = returnToPoolCallback;
    }

    public void PlayAt(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
        _particleSystem.Clear(); 
        _particleSystem.Play();
    }

    private void OnParticleSystemStopped()
    {
        onFinished?.Invoke(this);
    }
}