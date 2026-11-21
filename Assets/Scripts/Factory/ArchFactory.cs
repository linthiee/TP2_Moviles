using UnityEngine;

public class ArchFactory : MonoBehaviour
{
    [SerializeField] private Arch archPrefab;
    [SerializeField] private GameObject goalPrefab;
    [SerializeField] private Transform towerPivot;
    [SerializeField] private int initialPoolSize = 15;

    private ObjectPool<Arch> _archPool;
    private GameObject _goalInstance;

    private void Awake()
    {
        _archPool = new ObjectPool<Arch>(archPrefab, initialPoolSize, towerPivot);
    }

    public Arch CreateArch(FloorConfig config, float posY)
    {
        Arch arch = _archPool.Get();
        arch.transform.localPosition = new Vector3(0f, posY, 0f);
        arch.Configure(config);
        return arch;
    }

    public void ReleaseArch(Arch arch)
    {
        _archPool.Return(arch);
    }

    public GameObject CreateGoal(float posY)
    {
        if (_goalInstance == null)
        {
            _goalInstance = Instantiate(goalPrefab, towerPivot);
        }

        _goalInstance.transform.localPosition = new Vector3(0f, posY, 0f);
        _goalInstance.transform.localRotation = Quaternion.identity;
        _goalInstance.SetActive(true);
        
        return _goalInstance;
    }

    public void HideGoal()
    {
        if (_goalInstance != null)
            _goalInstance.SetActive(false);
    }
}