using System;

public class TutorialGenerationStrategy : IGenerationStrategy
{
    private readonly FloorConfig[] _floors;
    private int _currentFloor;

    public bool HasMoreFloors => _currentFloor < _floors.Length;

    public TutorialGenerationStrategy(int tutorialStep)
    {
        _floors = BuildStep(tutorialStep);
        Reset();
    }

    public void Reset()
    {
        _currentFloor = 0;
    }

    public FloorConfig GetNextFloor()
    {
        FloorConfig floor = _floors[_currentFloor];
        _currentFloor++;
        return floor;
    }

    private FloorConfig[] BuildStep(int step)
    {
        switch (step)
        {
            case 1:
                return new[]
                {
                    new FloorConfig
                    {
                        type = FloorType.Start, yRotation = 0.0f, activeSpikes = Array.Empty<int>(), coinIndex = -1,
                        hasBreakable = false
                    },
                    new FloorConfig
                    {
                        type = FloorType.Normal, yRotation = 90.0f, activeSpikes = Array.Empty<int>(), coinIndex = -1,
                        hasBreakable = false
                    },
                    new FloorConfig
                    {
                        type = FloorType.Goal, yRotation = 0.0f, activeSpikes = Array.Empty<int>(), coinIndex = -1,
                        hasBreakable = false
                    }
                };

            case 2:
                return new[]
                {
                    new FloorConfig
                    {
                        type = FloorType.Start, yRotation = 0.0f, activeSpikes = Array.Empty<int>(), coinIndex = -1,
                        hasBreakable = false
                    },
                    new FloorConfig
                    {
                        type = FloorType.Normal, yRotation = 45.0f, activeSpikes = new[] { 0, 1, 2 }, coinIndex = -1,
                        hasBreakable = false
                    },
                    new FloorConfig
                    {
                        type = FloorType.Normal, yRotation = 120.0f, activeSpikes = new[] { 3, 4, 5 }, coinIndex = -1,
                        hasBreakable = false
                    },
                    new FloorConfig
                    {
                        type = FloorType.Goal, yRotation = 0.0f, activeSpikes = Array.Empty<int>(), coinIndex = -1,
                        hasBreakable = false
                    }
                };

            default:
                return new[]
                {
                    new FloorConfig
                    {
                        type = FloorType.Normal, yRotation = 0.0f, activeSpikes = Array.Empty<int>(), coinIndex = -1,
                        hasBreakable = false
                    },
                    new FloorConfig
                    {
                        type = FloorType.Normal, yRotation = 0.0f, activeSpikes = Array.Empty<int>(), coinIndex = -1,
                        hasBreakable = false
                    },
                    new FloorConfig
                    {
                        type = FloorType.Normal, yRotation = 0.0f, activeSpikes = Array.Empty<int>(), coinIndex = -1,
                        hasBreakable = false
                    },
                    new FloorConfig
                    {
                        type = FloorType.Normal, yRotation = 0.0f, activeSpikes = Array.Empty<int>(), coinIndex = -1,
                        hasBreakable = true
                    },
                    new FloorConfig
                    {
                        type = FloorType.Goal, yRotation = 0.0f, activeSpikes = Array.Empty<int>(), coinIndex = -1,
                        hasBreakable = false
                    }
                };
        }
    }
}