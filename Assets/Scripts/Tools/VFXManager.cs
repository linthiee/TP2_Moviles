
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private PooledVFX archBreakPrefab;
    [SerializeField] private PooledVFX playerDeathPrefab;
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
    private ObjectPool<PooledVFX> _archBreakPool;
    private ObjectPool<PooledVFX> _playerDeathPool;
    private void OnEnable()
    {
        EventBus.Subscribe<OnSpikeCollision>(OnSpikeCollision);
        EventBus.Subscribe<OnArchBroken>(OnArchBroken);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<OnSpikeCollision>(OnSpikeCollision);
        EventBus.Unsubscribe<OnArchBroken>(OnArchBroken);
    }

    private void Awake()
    {
       _archBreakPool = new ObjectPool<PooledVFX>(archBreakPrefab, 4, transform);
        _playerDeathPool = new ObjectPool<PooledVFX>(playerDeathPrefab, 2, transform);
    }
    
    private void OnArchBroken(in OnArchBroken eventData)
    {
        PooledVFX vfx = _archBreakPool.Get();
        vfx.Initialize(_archBreakPool.Return);
        vfx.PlayAt(eventData.Position);
    }

    private void OnSpikeCollision(in OnSpikeCollision eventData)
    {
        PooledVFX vfx = _playerDeathPool.Get();
        vfx.Initialize(_playerDeathPool.Return);
        vfx.PlayAt(eventData.Position);
    }
}