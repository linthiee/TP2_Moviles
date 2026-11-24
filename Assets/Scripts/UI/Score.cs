using System.Collections;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text whiteHighlightScoreText;

    [SerializeField] private int baseArchPoints = 10;
    [SerializeField] private int archBreakBonus = 25;

    [SerializeField] private float punchScale = 1.25f;
    [SerializeField] private float punchDuration = 0.15f;

    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
    private GameSessionService Session => ServiceProvider.Instance.GetService<GameSessionService>();
    private GPGSManager GPGS => ServiceProvider.Instance.GetService<GPGSManager>();
    private bool IsTutorial => Session != null && Session.SelectedMode == GameMode.Tutorial;

    private int _score;
    private int _currentStreak;
    private Coroutine _punchCoroutine;
    private Vector3 _originalScoreScale;

    public int ActualScore => _score;

    private void Awake()
    {
        _originalScoreScale = scoreText.transform.localScale;
    }

    private void OnEnable()
    {
        EventBus.Subscribe<OnArchPassed>(OnArchPassed);
        EventBus.Subscribe<OnArchBroken>(OnArchBroken);
        EventBus.Subscribe<OnPlayerBounce>(OnPlayerBounce);
        EventBus.Subscribe<OnPlayerRespawn>(OnPlayerRespawn);
    }

    private void OnDisable()
    {
        ReportCurrentScore();
        
        EventBus.Unsubscribe<OnArchPassed>(OnArchPassed);
        EventBus.Unsubscribe<OnArchBroken>(OnArchBroken);
        EventBus.Unsubscribe<OnPlayerBounce>(OnPlayerBounce);
        EventBus.Unsubscribe<OnPlayerRespawn>(OnPlayerRespawn);
    }

    private void Start()
    {
        ResetScore();
    }

    private void OnArchPassed(in OnArchPassed eventData)
    {
        if (IsTutorial)
            return;

        _currentStreak++;
        int pointsToAdd = baseArchPoints * _currentStreak;
        AddScore(pointsToAdd);
    }

    private void OnArchBroken(in OnArchBroken eventData)
    {
        if (IsTutorial)
            return;

        AddScore(archBreakBonus);
        ResetStreakVisual();
    }

    private void OnPlayerBounce(in OnPlayerBounce eventData)
    {
        ResetStreakVisual();
    }

    private void OnPlayerRespawn(in OnPlayerRespawn eventData)
    {
        ReportCurrentScore();
        ResetScore();
    }

    private void ReportCurrentScore()
    {
        if (!IsTutorial && _score > 0)
            GPGS?.ReportScore(_score);
    }

    private void AddScore(int amount)
    {
        _score += amount;
        UpdateScoreText();

        if (_punchCoroutine != null)
            StopCoroutine(_punchCoroutine);
        _punchCoroutine = StartCoroutine(PunchScoreCoroutine());
    }

    private void ResetScore()
    {
        bool showScore = !IsTutorial;

        scoreText.gameObject.SetActive(showScore);
        whiteHighlightScoreText.gameObject.SetActive(showScore);

        _score = 0;
        UpdateScoreText();
        ResetStreakVisual();
    }

    private void ResetStreakVisual()
    {
        _currentStreak = 0;
    }

    private void UpdateScoreText()
    {
        scoreText.text = _score.ToString();
        whiteHighlightScoreText.text = _score.ToString();
    }

    private IEnumerator PunchScoreCoroutine()
    {
        Vector3 targetScale = _originalScoreScale * punchScale;
        float halfDuration = punchDuration * 0.5f;

        float elapsed = 0.0f;
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;

            scoreText.transform.localScale = Vector3.Lerp(_originalScoreScale, targetScale, elapsed / halfDuration);
            whiteHighlightScoreText.transform.localScale = Vector3.Lerp(_originalScoreScale, targetScale, elapsed / halfDuration);

            yield return null;
        }

        elapsed = 0.0f;
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;

            scoreText.transform.localScale = Vector3.Lerp(targetScale, _originalScoreScale, elapsed / halfDuration);
            whiteHighlightScoreText.transform.localScale = Vector3.Lerp(targetScale, _originalScoreScale, elapsed / halfDuration);

            yield return null;
        }

        scoreText.transform.localScale = _originalScoreScale;
        whiteHighlightScoreText.transform.localScale = _originalScoreScale;

        _punchCoroutine = null;
    }
}