using System;
using UnityEngine;

[Serializable]
public struct SkinData
{
    public string Id;
    public string DisplayName;
    public int Price;
    public string ResourcePath;
}

public class ShopService : IService
{
    private const string EquippedSkinKey = "Shop_EquippedSkin";
    private const string UnlockedPrefix = "Shop_Unlocked_";

    public readonly SkinData[] Catalog = new SkinData[]
    {
        new SkinData { Id = "default", DisplayName = "Classic", Price = 0,  ResourcePath = "" },
        new SkinData { Id = "atom",    DisplayName = "Atom",    Price = 5, ResourcePath = "Resources/AtomBall" },
        new SkinData { Id = "bucky",   DisplayName = "Bucky",   Price = 15, ResourcePath = "Resources/BuckyBall" },
        new SkinData { Id = "bomb",    DisplayName = "Bomb",    Price = 30, ResourcePath = "Resources/BombBall" },
        new SkinData { Id = "spike",   DisplayName = "Spike",   Price = 45, ResourcePath = "Resources/SpikeMetalBall" },
    };

    private GameObject _loadedSkinPrefab;

    private CurrencyService Currency => ServiceProvider.Instance.GetService<CurrencyService>();
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
    
    public bool IsPersistant => true;
    public string EquippedSkinId { get; private set; }
    
    public ShopService()
    {
        PlayerPrefs.SetInt(UnlockedPrefix + "default", 1);
        EquippedSkinId = PlayerPrefs.GetString(EquippedSkinKey, "default");
    }

    public bool IsUnlocked(string skinId)
    {
        return PlayerPrefs.GetInt(UnlockedPrefix + skinId, 0) == 1;
    }

    public bool TryBuyOrEquip(string skinId)
    {
        SkinData? target = FindSkin(skinId);
        if (target == null)
            return false;

        if (IsUnlocked(skinId))
        {
            Equip(skinId);
            return true;
        }

        if (!Currency.TrySpendCoins(target.Value.Price))
            return false;

        PlayerPrefs.SetInt(UnlockedPrefix + skinId, 1);
        PlayerPrefs.Save();

        Equip(skinId);
        return true;
    }

    public GameObject LoadEquippedSkinPrefab()
    {
        SkinData? skin = FindSkin(EquippedSkinId);
        if (skin == null || string.IsNullOrEmpty(skin.Value.ResourcePath))
            return null;

        if (_loadedSkinPrefab != null)
            return _loadedSkinPrefab;

        _loadedSkinPrefab = Resources.Load<GameObject>(skin.Value.ResourcePath);
        return _loadedSkinPrefab;
    }

    public void UnloadCurrentSkin()
    {
        if (_loadedSkinPrefab == null)
            return;

        _loadedSkinPrefab = null;
        Resources.UnloadUnusedAssets();
    }

    private void Equip(string skinId)
    {
        if (EquippedSkinId == skinId)
            return;

        UnloadCurrentSkin();

        EquippedSkinId = skinId;
        PlayerPrefs.SetString(EquippedSkinKey, skinId);
        PlayerPrefs.Save();

        EventBus.Raise<OnShopUpdated>();
    }

    private SkinData? FindSkin(string skinId)
    {
        for (int i = 0; i < Catalog.Length; i++)
        {
            if (Catalog[i].Id == skinId)
                return Catalog[i];
        }

        return null;
    }
}