using System;

public class LevelGenerationStrategy : IGenerationStrategy
{
    private readonly int _levelNumber;
    private readonly int _totalFloors;
    private readonly int _spikeSlots;
    private readonly int _coinSlots;

    private Random _random;
    private int _currentFloor;
    private float _lastRotationY;

    public bool HasMoreFloors => _currentFloor < _totalFloors;

    public LevelGenerationStrategy(int levelNumber, int totalFloors, int spikeSlots, int coinSlots)
    {
        _levelNumber = levelNumber;
        _totalFloors = totalFloors;
        _spikeSlots = spikeSlots;
        _coinSlots = coinSlots;
        Reset();
    }

    public void Reset()
    {
        _random = new Random(_levelNumber);
        _currentFloor = 0;
        _lastRotationY = 0.0f;
    }

    public FloorConfig GetNextFloor()
    {
        int floorIndex = _currentFloor;
        _currentFloor++;

        if (floorIndex == 0)
        {
            FloorConfig config;

            config.type = FloorType.Start;
            config.yRotation = 0.0f;
            config.activeSpikes = Array.Empty<int>();
            config.coinIndex = -1;
            config.hasBreakable = false;

            return config;
        }

        if (floorIndex >= _totalFloors - 1)
        {
            FloorConfig config;

            config.type = FloorType.Goal;
            config.yRotation = 0.0f;
            config.activeSpikes = Array.Empty<int>();
            config.coinIndex = -1;
            config.hasBreakable = false;

            return config;
        }

        int delta = 0;
        if (_random.NextDouble() < 0.25)
        {
            delta = _random.Next(-15, 16);
        }
        else
        {
            int direction = _random.Next(2) == 0 ? 1 : -1;
            delta = _random.Next(20, 60) * direction;
        }

        _lastRotationY = (_lastRotationY + delta + 360f) % 360.0f;
        int maxSpikes = Math.Min(_spikeSlots / 2, 1 + _levelNumber);
        int spikeCount = _random.Next(1, maxSpikes + 1);

        int coinIndex = _random.NextDouble() < 0.4 ? _random.Next(0, _coinSlots) : -1;

        FloorConfig normalFloorConfig;

        normalFloorConfig.type = FloorType.Normal;
        normalFloorConfig.yRotation = _lastRotationY;
        normalFloorConfig.activeSpikes = PickUniqueIndices(spikeCount, _spikeSlots);
        normalFloorConfig.coinIndex = coinIndex;

        normalFloorConfig.hasBreakable = _random.NextDouble() < 0.35;
#if UNITY_EDITOR
        normalFloorConfig.hasBreakable = false;
#endif
        return normalFloorConfig;
    }

    private int[] PickUniqueIndices(int count, int maxSlots)
    {
        count = Math.Min(count, maxSlots);
        if (count <= 0)
            return Array.Empty<int>();

        int[] result = new int[count];
        int picked = 0;

        while (picked < count)
        {
            int candidate = _random.Next(0, maxSlots);
            if (Array.IndexOf(result, candidate, 0, picked) == -1)
            {
                result[picked] = candidate;
                picked++;
            }
        }

        return result;
    }
}