
public interface IGenerationStrategy
{
    bool HasMoreFloors { get; }
    FloorConfig GetNextFloor();
    void Reset();
}