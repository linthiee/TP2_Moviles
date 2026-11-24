using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Play Buttons")]
    [SerializeField] private Button playLevelButton;
    [SerializeField] private TMP_Text playLevelButtonText;
    [SerializeField] private Button playEndlessButton;
    [SerializeField] private Button tutorialButton;

    [Header("Panel")]
    [SerializeField] private GameObject logPanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject shopPanel;
    
    [Header("Feature Buttons")]
    [SerializeField] private Button shopButton;
    [SerializeField] private Button achievementsButton;
    [SerializeField] private Button leaderboardButton;
    [SerializeField] private Button loggerButton;

    [Header("Scene")]
    [SerializeField] private string gameplaySceneName = "Super Bouncy Ball";

    private GameSessionService Session => ServiceProvider.Instance.GetService<GameSessionService>();

    private void Start()
    {
        if (playLevelButtonText != null && Session != null)
            playLevelButtonText.text = $"Level {Session.CurrentLevel}";
    }

    private void OnEnable()
    {
        playLevelButton?.onClick.AddListener(OnPlayLevelClicked);
        playEndlessButton?.onClick.AddListener(OnPlayEndlessClicked);
        tutorialButton?.onClick.AddListener(OnTutorialClicked);

        shopButton?.onClick.AddListener(OnShopClicked);
        achievementsButton?.onClick.AddListener(OnAchievementsClicked);
        leaderboardButton?.onClick.AddListener(OnLeaderboardClicked);
        loggerButton?.onClick.AddListener(OnLoggerClicked);
    }

    private void OnDisable()
    {
        playLevelButton?.onClick.RemoveListener(OnPlayLevelClicked);
        playEndlessButton?.onClick.RemoveListener(OnPlayEndlessClicked);
        tutorialButton?.onClick.RemoveListener(OnTutorialClicked);

        shopButton?.onClick.RemoveListener(OnShopClicked);
        achievementsButton?.onClick.RemoveListener(OnAchievementsClicked);
        leaderboardButton?.onClick.RemoveListener(OnLeaderboardClicked);
        loggerButton?.onClick.RemoveListener(OnLoggerClicked);
    }

    private void OnPlayLevelClicked()
    {
        Session.SelectLevelMode();
        LoadGameplay();
    }

    private void OnPlayEndlessClicked()
    {
        Session.SelectEndlessMode();
        LoadGameplay();
    }

    private void OnTutorialClicked()
    {
        Session.SelectTutorialMode();
        LoadGameplay();
    }
    private void OnShopClicked()
    {
        mainMenuPanel.SetActive(false);
        shopPanel.SetActive(true);
    }

    private void OnAchievementsClicked()
    {
        Debug.Log("[MainMenu] Open achievements");
    }

    private void OnLeaderboardClicked()
    {
        Debug.Log("[MainMenu] Open leaderboard");
    }

    private void OnLoggerClicked()
    {
        mainMenuPanel.SetActive(false);
        logPanel.SetActive(true);
    }

    private void LoadGameplay()
    {
        SceneManager.LoadSceneAsync(gameplaySceneName);
    }
}