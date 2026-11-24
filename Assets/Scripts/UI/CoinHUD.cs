using TMPro;
using UnityEngine;

public class CoinHUD : MonoBehaviour
{
    private TMP_Text _coinsText;

    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
    private CurrencyService Currency => ServiceProvider.Instance.GetService<CurrencyService>();

    private void Awake()
    {
        TryGetComponent(out _coinsText);
    }

    private void OnEnable()
    {
        EventBus.Subscribe<OnCoinsChanged>(OnCoinsChanged);

        if (Currency != null)
            UpdateText(Currency.Coins);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<OnCoinsChanged>(OnCoinsChanged);
    }

    private void OnCoinsChanged(in OnCoinsChanged eventData)
    {
        UpdateText(eventData.Coins);
    }

    private void UpdateText(int amount)
    {
        _coinsText.text = $"Coins: {amount}";
    }
}