using System;
using UnityEngine;

public class CurrencyService : IService
{
    private const string CoinsKey = "Player_Coins";

    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    public bool IsPersistant => true;
    public int Coins { get; private set; } = PlayerPrefs.GetInt(CoinsKey, 0);

    public CurrencyService()
    {
        EventBus.Subscribe<OnCoinCollected>(OnCoinCollected);
    }

    public void AddCoins(int amount)
    {
        if (amount <= 0)
            return;

        Coins += amount;
        Save();
        EventBus.Raise<OnCoinsChanged>(Coins);
    }

    public bool TrySpendCoins(int amount)
    {
        if (amount < 0 || Coins < amount)
            return false;

        Coins -= amount;
        Save();
        EventBus.Raise<OnCoinsChanged>(Coins);  
        return true;
    }

    private void OnCoinCollected(in OnCoinCollected eventData)
    {
        AddCoins(1);
    }

    private void Save()
    {
        PlayerPrefs.SetInt(CoinsKey, Coins);
        PlayerPrefs.Save();
    }
}