using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogPanel : MonoBehaviour
{
    [Header("Panel")] [SerializeField] private GameObject mainMenuPanel;

    [Header("Display")] [SerializeField] private TMP_Text logsText;

    [Header("Buttons")] [SerializeField] private Button getAllLogsButton;
    [SerializeField] private Button refreshButton;
    [SerializeField] private Button clearButton;
    [SerializeField] private Button testLogButton;
    [SerializeField] private Button closeButton;

    private MousseLogger _logger;

    private void Start()
    {
        _logger = ServiceProvider.Instance.GetService<MousseLogger>();
    }

    private void OnEnable()
    {
        getAllLogsButton?.onClick.AddListener(RefreshLogs);
        refreshButton?.onClick.AddListener(RefreshLogs);
        clearButton?.onClick.AddListener(OnClearClicked);
        testLogButton?.onClick.AddListener(OnTestLogClicked);
        closeButton?.onClick.AddListener(ClosePanel);

        if (_logger != null)
            _logger.LogsUpdated += RefreshLogs;

        RefreshLogs();
    }

    private void OnDisable()
    {
        getAllLogsButton?.onClick.RemoveListener(RefreshLogs);
        refreshButton?.onClick.RemoveListener(RefreshLogs);
        clearButton?.onClick.RemoveListener(OnClearClicked);
        testLogButton?.onClick.RemoveListener(OnTestLogClicked);
        closeButton?.onClick.RemoveListener(ClosePanel);

        if (_logger != null)
            _logger.LogsUpdated -= RefreshLogs;
    }

    private void RefreshLogs()
    {
        if (logsText != null && _logger != null)
            logsText.text = _logger.GetAllLogs();
    }

    private void OnClearClicked()
    {
        _logger?.RequestClearLogs();
    }

    private void OnTestLogClicked()
    {
        _logger?.GenerateTestLog();
        RefreshLogs();
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}