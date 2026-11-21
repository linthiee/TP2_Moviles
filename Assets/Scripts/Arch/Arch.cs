using UnityEngine;

public class Arch : MonoBehaviour
{
    [SerializeField] private Breakable _breakable;
    [SerializeField] private ArchCollider _collider;
    
    [SerializeField] private GameObject[] _spikes;
    [SerializeField] private GameObject[] _coins;

    private void Awake()
    {
        _breakable = GetComponentInChildren<Breakable>(true);
    }

    public void Configure(FloorConfig config)
    {
        _breakable.gameObject.SetActive(config.hasBreakable);
        if (config.hasBreakable)
            _breakable.ResetState();
        
        _collider.ResetTrigger();
        
        transform.localRotation = Quaternion.Euler(0f, config.yRotation, 0f);

        for (int i = 0; i < _spikes.Length; i++)
            _spikes[i].SetActive(false);

        if (config.activeSpikes != null)
        {
            for (int i = 0; i < config.activeSpikes.Length; i++)
            {
                int index = config.activeSpikes[i];
                if (index >= 0 && index < _spikes.Length)
                    _spikes[index].SetActive(true);
            }
        }

        for (int i = 0; i < _coins.Length; i++)
            _coins[i].SetActive(false);

        if (config.coinIndex >= 0 && config.coinIndex < _coins.Length)
            _coins[config.coinIndex].SetActive(true);
    }
}