using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    [SerializeField] private GameObject shopPanel;

    [SerializeField] private bool pauseGameOnOpen = true;

    private Button _button;

    private void Awake()
    {
        TryGetComponent(out _button);
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OpenShop);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OpenShop);
    }

    private void OpenShop()
    {
        if (pauseGameOnOpen)
            Time.timeScale = 0f;

        shopPanel.SetActive(true);
    }
}