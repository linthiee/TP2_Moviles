using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameplayFlowController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TowerGenerator towerGenerator;
    [SerializeField] private Player player;

    [Header("Tutorial UI")]
    [SerializeField] private GameObject tutorialBanner;
    [SerializeField] private TMP_Text tutorialText;

    [Header("Pause UI")]
    [SerializeField] private Button pauseButton;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    [Header("Settings")]
    [SerializeField] private string mainMenuSceneName = "Main Menu";
    [SerializeField] private float delayAfterWin = 1.5f;
    [SerializeField] private int spikeSlots = 14;
    [SerializeField] private int coinSlots = 3;
    [SerializeField] private int baseFloorsPerLevel = 15;

    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
    private GameSessionService Session => ServiceProvider.Instance.GetService<GameSessionService>();

    private bool _isPaused;

    private void OnEnable()
    {
        EventBus.Subscribe<OnPlayerWin>(HandlePlayerWin);

        pauseButton?.onClick.AddListener(TogglePause);
        resumeButton?.onClick.AddListener(ResumeGame);
        restartButton?.onClick.AddListener(RestartLevel);
        mainMenuButton?.onClick.AddListener(GoToMainMenu);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<OnPlayerWin>(HandlePlayerWin);

        pauseButton?.onClick.RemoveListener(TogglePause);
        resumeButton?.onClick.RemoveListener(ResumeGame);
        restartButton?.onClick.RemoveListener(RestartLevel);
        mainMenuButton?.onClick.RemoveListener(GoToMainMenu);
    }

    private void Start()
    {
        Time.timeScale = 1.0f;
        pausePanel.SetActive(false);

        UpdateTutorialUI();
    }

    private void HandlePlayerWin(in OnPlayerWin eventData)
    {
        ServiceProvider.Instance.GetService<TaskScheduler>().Schedule(AdvanceAfterWin, delayAfterWin);
    }

    private void AdvanceAfterWin()
    {
        if (Session.SelectedMode == GameMode.Tutorial)
        {
            if (Session.TutorialStep < 3)
            {
                Session.AdvanceTutorialStep();
                LoadNextStrategy(new TutorialGenerationStrategy(Session.TutorialStep));
            }
            else
            {
                Session.CompleteTutorial();
                int totalFloors = baseFloorsPerLevel + (Session.CurrentLevel * 2);
                LoadNextStrategy(new LevelGenerationStrategy(Session.CurrentLevel, totalFloors, spikeSlots, coinSlots));
            }
        }
        else if (Session.SelectedMode == GameMode.Level)
        {
            Session.AdvanceLevel();
            int totalFloors = baseFloorsPerLevel + (Session.CurrentLevel * 2);
            LoadNextStrategy(new LevelGenerationStrategy(Session.CurrentLevel, totalFloors, spikeSlots, coinSlots));
        }

        UpdateTutorialUI();
    }

    private void LoadNextStrategy(IGenerationStrategy nextStrategy)
    {
        towerGenerator.BuildTower(nextStrategy);
        player.Respawm();
    }

    private void UpdateTutorialUI()
    {
        if (Session.SelectedMode != GameMode.Tutorial)
        {
            tutorialBanner.SetActive(false);
            return;
        }

        tutorialBanner.SetActive(true);

        switch (Session.TutorialStep)
        {
            case 1:
                tutorialText.text = "Drag to make the tower spin and make the ball fall!";
                break;
            case 2:
                tutorialText.text = "Be careful! Dont touch the spikes";
                break;
            case 3:
                tutorialText.text = "Fall 3 floors in a row to instantly break the piece thats a different color! If you fail, you will have to jump on it 3 times";
                break;
        }
    }

    public void TogglePause()
    {
        if (_isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    private void PauseGame()
    {
        _isPaused = true;
        Time.timeScale = 0.0f;
        pausePanel.SetActive(true);
    }

    private void ResumeGame()
    {
        _isPaused = false;
        Time.timeScale = 1.0f;
        pausePanel.SetActive(false);
    }

    private void RestartLevel()
    {
        ResumeGame();
        player.Respawm();
    }

    private void GoToMainMenu()
    {
        Time.timeScale = 1.0f;

        SceneManager.LoadSceneAsync(mainMenuSceneName);
    }
}