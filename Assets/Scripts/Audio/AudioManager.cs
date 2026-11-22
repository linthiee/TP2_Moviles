using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource streakSource;
    
    [SerializeField] private AudioClip bounceClip;
    [SerializeField] private AudioClip archPassedClip;
    [SerializeField] private AudioClip playerDeathClip;
    [SerializeField] private AudioClip winClip;
    [SerializeField] private AudioClip coinCollectedClip;
    
    private float _basePitch = 1.0f;
    private float _pitchStep = 0.1f; 
    private float _maxPitch = 2.0f;

    private Player _player;    
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private void OnEnable()
    {
        EventBus.Subscribe<OnPlayerBounce>(OnPlayerBounce);
        EventBus.Subscribe<OnArchPassed>(OnArchPassed);
        EventBus.Subscribe<OnArchBroken>(OnArchBroken);
        EventBus.Subscribe<OnSpikeCollision>(OnSpikeCollision);
        EventBus.Subscribe<OnPlayerWin>(OnPlayerWin);
        EventBus.Subscribe<OnCoinCollected>(OnCoinCollected);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<OnPlayerBounce>(OnPlayerBounce);
        EventBus.Unsubscribe<OnArchPassed>(OnArchPassed);
        EventBus.Unsubscribe<OnSpikeCollision>(OnSpikeCollision);
        EventBus.Unsubscribe<OnPlayerWin>(OnPlayerWin);
        EventBus.Unsubscribe<OnCoinCollected>(OnCoinCollected);
    }
    
    private void Start()
    {
        _player = FindFirstObjectByType(typeof(Player)) as Player;
    }
    
    private void OnPlayerBounce(in OnPlayerBounce eventData)
    {
        sfxSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
        sfxSource.PlayOneShot(bounceClip);
    }
    
    private void OnArchPassed(in OnArchPassed eventData)
    {
        float targetPitch = Mathf.Min(_basePitch + _player.Streak * _pitchStep, _maxPitch);

        streakSource.pitch = targetPitch;
        streakSource.PlayOneShot(archPassedClip);
    }

    private void OnArchBroken(in OnArchBroken eventData)
    {
        float targetPitch = Mathf.Min(_basePitch + _player.Streak * _pitchStep, _maxPitch);

        streakSource.pitch = targetPitch;
        streakSource.PlayOneShot(archPassedClip);
    }
    private void OnSpikeCollision(in OnSpikeCollision eventData)
    {
        sfxSource.PlayOneShot(playerDeathClip);
    }
    private void OnPlayerWin(in OnPlayerWin eventData)
    {
        Debug.Log("Playing win audio clip");
        sfxSource.PlayOneShot(winClip);
    }
    private void OnCoinCollected(in OnCoinCollected eventData)
    {
     sfxSource.PlayOneShot(coinCollectedClip);
    }

}