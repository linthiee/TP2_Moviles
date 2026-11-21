using System.Collections.Generic;
using UnityEngine;

public class TowerGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ArchFactory ringFactory;

    [Header("Tower Dimensions")]
    [SerializeField] private float startY = 0.0f;
    [SerializeField] private float archSpacing = 2.5f;
    [SerializeField] private int initialVisibleFloors = 10;
    [SerializeField] private int floorsKeptAbove = 2;

    [Header("Arch Prefab Slots")]
    [SerializeField] private int spikeSlots = 14;
    [SerializeField] private int coinSlots = 3;

    [Header("For testing without menu)")]
    [SerializeField] private bool testEndlessMode = false;
    [SerializeField] private int testLevelNumber = 1;
    [SerializeField] private int baseFloorsPerLevel = 15;

    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private IGenerationStrategy _strategy;
    private readonly Queue<Arch> _activeArches = new Queue<Arch>();
    private float _nextSpawnY;
    private int _passedCount;

    private void OnEnable()
    {
        EventBus.Subscribe<OnArchPassed>(HandleArchPassed);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<OnArchPassed>(HandleArchPassed);
    }

    private void Start()
    {
        IGenerationStrategy strategy = testEndlessMode ? new EndlessGenerationStrategy(spikeSlots, coinSlots) :
            new LevelGenerationStrategy(testLevelNumber, baseFloorsPerLevel + (testLevelNumber * 2), spikeSlots, coinSlots);

        BuildTower(strategy);
    }

    public void BuildTower(IGenerationStrategy strategy)
    {
        ClearTower();

        _strategy = strategy;
        _strategy.Reset();

        _nextSpawnY = startY;
        _passedCount = 0;

        for (int i = 0; i < initialVisibleFloors; i++)
        {
            SpawnNextFloor();
        }
    }

    public void RestartCurrentTower()
    {
        if (_strategy != null)
            BuildTower(_strategy);
    }

    private void HandleArchPassed(in OnArchPassed eventData)
    {
        _passedCount++;

        if (_passedCount > floorsKeptAbove && _activeArches.Count > 0)
        {
            Arch oldest = _activeArches.Dequeue();
            ringFactory.ReleaseArch(oldest);
        }

        SpawnNextFloor();
    }

    private void SpawnNextFloor()
    {
        if (_strategy == null || !_strategy.HasMoreFloors)
            return;

        FloorConfig config = _strategy.GetNextFloor();

        if (config.type == FloorType.Goal)
        {
            ringFactory.CreateGoal(_nextSpawnY);
        }
        else
        {
            Arch arch = ringFactory.CreateArch(config, _nextSpawnY);
            _activeArches.Enqueue(arch);
        }

        _nextSpawnY -= archSpacing;
    }

    private void ClearTower()
    {
        while (_activeArches.Count > 0)
        {
            ringFactory.ReleaseArch(_activeArches.Dequeue());
        }

        ringFactory.HideGoal();
    }
}