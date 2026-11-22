using System.Collections.Generic;
using UnityEngine;

public class TowerGenerator : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private ArchFactory ringFactory;

    [Header("Tower Dimensions")] [SerializeField]
    private float startY = 0.0f;

    [SerializeField] private float archSpacing = 0.55f;
    [SerializeField] private int initialVisibleFloors = 10;
    [SerializeField] private int floorsKeptAbove = 2;

    [Header("Arch Prefab Slots")] [SerializeField]
    private int spikeSlots = 14;

    [SerializeField] private int coinSlots = 3;

    [Header("For testing without menu)")] [SerializeField]
    private bool testEndlessMode = false;

    [SerializeField] private int testLevelNumber = 1;
    [SerializeField] private int baseFloorsPerLevel = 15;

    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private IGenerationStrategy _strategy;
    private readonly Queue<Arch> _activeArches = new Queue<Arch>();
    private float _nextSpawnY;
    private int _passedCount;

    private void OnEnable()
    {
        EventBus.Subscribe<OnArchPassed>(OnArchPassed);
        EventBus.Subscribe<OnPlayerRespawn>(OnPlayerRespawn);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<OnArchPassed>(OnArchPassed);
        EventBus.Unsubscribe<OnPlayerRespawn>(OnPlayerRespawn);
    }

    private void Start()
    {
        IGenerationStrategy strategy;
        GameSessionService session = ServiceProvider.Instance.GetService<GameSessionService>();

        switch (session.SelectedMode)
        {
            case GameMode.Endless:
                strategy = new EndlessGenerationStrategy(spikeSlots, coinSlots);
                break;

            case GameMode.Tutorial:
                strategy = new TutorialGenerationStrategy(session.TutorialStep);
                break;

            case GameMode.Level:
            default:
                int totalFloors = baseFloorsPerLevel + (session.CurrentLevel * 2);
                strategy = new LevelGenerationStrategy(session.CurrentLevel, totalFloors, spikeSlots, coinSlots);
                break;
        }


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

    private void OnArchPassed(in OnArchPassed eventData)
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

    private void OnPlayerRespawn(in OnPlayerRespawn eventData)
    {
        RestartCurrentTower();
    }
}