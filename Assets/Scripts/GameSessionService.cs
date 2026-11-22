using UnityEngine;

public enum GameMode
{
    Tutorial,
    Level,
    Endless
}

public class GameSessionService : IService
{
    private const string CurrentLevelKey = "CurrentLevel";
    private const string TutorialDoneKey = "TutorialDone";

    public bool IsPersistant => true;

    public GameMode SelectedMode { get; private set; } = GameMode.Level;
    public int CurrentLevel { get; private set; }
    public int TutorialStep { get; private set; } = 1;
    public bool IsTutorialCompleted { get; private set; }

    public GameSessionService()
    {
        CurrentLevel = PlayerPrefs.GetInt(CurrentLevelKey, 1);
        int savedValue = PlayerPrefs.GetInt(TutorialDoneKey, 0);

        if (savedValue == 1)
            IsTutorialCompleted = true;
        else
            IsTutorialCompleted = false;
    }

    public void SelectLevelMode()
    {
        if (!IsTutorialCompleted)
        {
            SelectTutorialMode();
            return;
        }

        SelectedMode = GameMode.Level;
    }

    public void SelectEndlessMode()
    {
        SelectedMode = GameMode.Endless;
    }

    public void SelectTutorialMode()
    {
        SelectedMode = GameMode.Tutorial;
        TutorialStep = 1;
    }

    public void AdvanceLevel()
    {
        CurrentLevel++;
        PlayerPrefs.SetInt(CurrentLevelKey, CurrentLevel);
        PlayerPrefs.Save();
    }

    public void AdvanceTutorialStep()
    {
        TutorialStep++;
    }

    public void CompleteTutorial()
    {
        IsTutorialCompleted = true;
        PlayerPrefs.SetInt(TutorialDoneKey, 1);
        PlayerPrefs.Save();
        SelectedMode = GameMode.Level;
    }
}