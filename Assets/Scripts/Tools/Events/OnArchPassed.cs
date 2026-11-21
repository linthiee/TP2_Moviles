
public struct OnArchPassed : IEvent
{
    public float ArchPosY; 
    public void Assign(params object[] parameters)
    {
        ArchPosY = (float)parameters[0];
    }

    public void Reset()
    {
        ArchPosY = 0.0f;
    }
}
