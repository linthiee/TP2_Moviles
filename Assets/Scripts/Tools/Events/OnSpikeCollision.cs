using UnityEngine;

public struct OnSpikeCollision : IEvent
{
    public Vector3 Position;
    public void Assign(params object[] parameters)
    {
        Position = (Vector3)parameters[0];
    }

    public void Reset()
    {
        Position = Vector3.zero;
    }
}