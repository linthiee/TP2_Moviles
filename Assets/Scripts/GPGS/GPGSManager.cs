using UnityEngine;
#if UNITY_ANDROID && !UNITY_EDITOR
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

public class GPGSManager : IService
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    public bool IsPersistant => true;
    public bool IsAuthenticated { get; private set; }

    public GPGSManager()
    {
        Authenticate();
        SubscribeToEvents();
    }

    public void Authenticate()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
#endif
    }

    public void SignInManually()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication);
#endif
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    private void ProcessAuthentication(SignInStatus status)
    {
        IsAuthenticated = status == SignInStatus.Success;
        Debug.Log($"[GPGS] Login: {status}");
    }
#endif

    public void ReportScore(long score)
    {
        if (!IsAuthenticated || score <= 0)
            return;

#if UNITY_ANDROID && !UNITY_EDITOR
     PlayGamesPlatform.Instance.ReportScore(score, GPGSIds.leaderboard_highscore, ok =>
        Debug.Log($"[GPGS] ReportScore ({score}): {ok}"));
#endif
    }

    public void UnlockAchievement(string achievementId)
    {
        if (!IsAuthenticated || string.IsNullOrEmpty(achievementId))
            return;

#if UNITY_ANDROID && !UNITY_EDITOR
        PlayGamesPlatform.Instance.ReportProgress(achievementId, 100.0, ok =>
            Debug.Log($"[GPGS] Logro {achievementId}: {ok}"));
#endif
    }

    public void ShowLeaderboard()
    {
        if (!IsAuthenticated)
        {
            SignInManually();
            return;
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_highscore);
#endif
    }

    public void ShowAchievements()
    {
        if (!IsAuthenticated)
        {
            SignInManually();
            return;
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        PlayGamesPlatform.Instance.ShowAchievementsUI();
#endif
    }

    private void SubscribeToEvents()
    {
        if (EventBus == null)
            return;

        EventBus.Subscribe<OnArchBroken>(OnArchBroken);
        EventBus.Subscribe<OnShopUpdated>(OnShopUpdated);
    }

    private void OnArchBroken(in OnArchBroken eventData)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
    UnlockAchievement(GPGSIds.achievement_arch_breaker);
#endif
    }

    private void OnShopUpdated(in OnShopUpdated eventData)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
    UnlockAchievement(GPGSIds.achievement_in_style);
#endif
    }
}