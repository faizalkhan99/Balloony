using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using Unity.Services;
using System.Collections.Generic;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance;

    [System.Obsolete]
    async void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        try
        {
            await UnityServices.InitializeAsync();
            Debug.Log("Unity Services Initialized");

            // Consent is required. In a real app, show a UI.
            // For now, we assume consent to track.
            AnalyticsService.Instance.StartDataCollection();
            Debug.Log("Analytics Data Collection Started");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to initialize Unity Services: {e.Message}");
        }
    }

    // --- CUSTOM EVENTS ---

    public void LogGameStart()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized) return;

        Debug.Log("Analytics: Game Start Event Sent");

        // FIX: Create a CustomEvent object
        CustomEvent gameStartEvent = new CustomEvent("match_started")
        {
            { "platform", Application.platform.ToString() }
        };

        // FIX: Use RecordEvent
        AnalyticsService.Instance.RecordEvent(gameStartEvent);
    }

    public void LogGameOver(int score, int highScore, bool isNewHighScore)
    {
        if (UnityServices.State != ServicesInitializationState.Initialized) return;

        Debug.Log($"Analytics: Game Over. Score: {score}");

        // FIX: Create a CustomEvent object
        CustomEvent gameOverEvent = new CustomEvent("game_over")
        {
            { "score", score },
            { "high_score", highScore },
            { "is_new_high_score", isNewHighScore }
        };

        // FIX: Use RecordEvent
        AnalyticsService.Instance.RecordEvent(gameOverEvent);
        
        AnalyticsService.Instance.Flush();
    }
}