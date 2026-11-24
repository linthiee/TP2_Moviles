using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{
    [Header("Panels")] [SerializeField] private GameObject returnPanel;

    [Header("Header")] [SerializeField] private TMP_Text coinsText;
    [SerializeField] private Button closeButton;

    [Header("Skin Buttons")] [SerializeField]
    private Button[] skinButtons;

    [SerializeField] private TMP_Text[] skinButtonTexts;

    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
    private CurrencyService Currency => ServiceProvider.Instance.GetService<CurrencyService>();
    private ShopService Shop => ServiceProvider.Instance.GetService<ShopService>();

    private void OnEnable()
    {
        closeButton?.onClick.AddListener(ClosePanel);

        for (int i = 0; i < skinButtons.Length; i++)
        {
            int index = i;
            skinButtons[i]?.onClick.AddListener(() => OnSkinClicked(index));
        }

        EventBus.Subscribe<OnCoinsChanged>(OnCoinsChanged);
        EventBus.Subscribe<OnShopUpdated>(OnShopUpdated);

        RefreshUI();
    }

    private void OnDisable()
    {
        closeButton?.onClick.RemoveListener(ClosePanel);

        for (int i = 0; i < skinButtons.Length; i++)
            skinButtons[i]?.onClick.RemoveAllListeners();

        EventBus.Unsubscribe<OnCoinsChanged>(OnCoinsChanged);
        EventBus.Unsubscribe<OnShopUpdated>(OnShopUpdated);
    }

    private void OnSkinClicked(int index)
    {
        if (index >= Shop.Catalog.Length)
            return;

        Shop.TryBuyOrEquip(Shop.Catalog[index].Id);
    }

    private void OnCoinsChanged(in OnCoinsChanged eventData)
    {
        UpdateCoinsUI(eventData.Coins);
        RefreshUI();
    }

    private void OnShopUpdated(in OnShopUpdated eventData)
    {
        RefreshUI();
    }

    private void RefreshUI()
    {
        UpdateCoinsUI(Currency.Coins);

        if (Shop == null)
            return;

        for (int i = 0; i < skinButtons.Length && i < Shop.Catalog.Length; i++)
        {
            SkinData skin = Shop.Catalog[i];
            bool unlocked = Shop.IsUnlocked(skin.Id);
            bool equipped = Shop.EquippedSkinId == skin.Id;

            if (equipped)
                skinButtonTexts[i].text = $"{skin.DisplayName} (Equipped)";
            else if (unlocked)
                skinButtonTexts[i].text = $"{skin.DisplayName} (Equip)";
            else
                skinButtonTexts[i].text = $"{skin.DisplayName} - {skin.Price} Coins";

            skinButtons[i].interactable = !equipped && (unlocked || (Currency != null && Currency.Coins >= skin.Price));
        }
    }

    private void UpdateCoinsUI(int amount)
    {
        if (coinsText != null)
            coinsText.text = $"Coins: {amount}";
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);

        if (returnPanel != null)
            returnPanel.SetActive(true);

        Time.timeScale = 1.0f;
    }
}