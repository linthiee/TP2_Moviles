using UnityEngine;

public class SkinApplier : MonoBehaviour
{
    [SerializeField] private MeshRenderer defaultBallRenderer;

    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
    private ShopService Shop => ServiceProvider.Instance.GetService<ShopService>();

    private GameObject _currentSkinInstance;

    private void OnEnable()
    {
        EventBus.Subscribe<OnSpikeCollision>(OnSpikeCollision);
        EventBus.Subscribe<OnPlayerRespawn>(OnPlayerRespawn);
        EventBus.Subscribe<OnShopUpdated>(OnShopUpdated);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<OnSpikeCollision>(OnSpikeCollision);
        EventBus.Unsubscribe<OnPlayerRespawn>(OnPlayerRespawn);
        EventBus.Unsubscribe<OnShopUpdated>(OnShopUpdated);
    }

    private void Start()
    {
        if (defaultBallRenderer == null)
            TryGetComponent(out defaultBallRenderer);

        ApplyEquippedSkin();
    }

    private void OnShopUpdated(in OnShopUpdated eventData)
    {
        ApplyEquippedSkin();
    }

    private void ApplyEquippedSkin()
    {
        if (_currentSkinInstance != null)
        {
            Destroy(_currentSkinInstance);
            _currentSkinInstance = null;
        }

        if (Shop == null)
            return;

        GameObject skinPrefab = Shop.LoadEquippedSkinPrefab();

        if (skinPrefab == null)
        {
            if (defaultBallRenderer != null)
                defaultBallRenderer.enabled = true;
            return;
        }

        if (defaultBallRenderer != null)
            defaultBallRenderer.enabled = false;

        _currentSkinInstance = Instantiate(skinPrefab, transform);
        _currentSkinInstance.transform.localPosition = Vector3.zero;
        _currentSkinInstance.transform.localRotation = Quaternion.identity;
    }

    private void OnSpikeCollision(in OnSpikeCollision eventData)
    {
        if (_currentSkinInstance != null)
            _currentSkinInstance.SetActive(false);
    }

    private void OnPlayerRespawn(in OnPlayerRespawn eventData)
    {
        if (_currentSkinInstance != null)
        {
            _currentSkinInstance.SetActive(true);

            if (defaultBallRenderer != null)
                defaultBallRenderer.enabled = false;
        }
    }
}