public enum FloorType
{
    Start,
    Normal,
    Goal
}

public struct FloorConfig
{
   public FloorType type;
   public float yRotation;
   public int[] activeSpikes;
   public int coinIndex;
   public bool hasBreakable;
}