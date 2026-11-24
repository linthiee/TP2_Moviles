public struct OnCoinsChanged : IEvent
{
    public int Coins;

    public void Assign(params object[] parameters)
    {
        Coins = (int)parameters[0];
    }

    public void Reset()
    {
        Coins = 0;
    }
}