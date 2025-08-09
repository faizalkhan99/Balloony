using UnityEngine;
using System;
using System.Collections;

public class GameCountdownManager : MonoBehaviour
{
    public static GameCountdownManager Instance { get; private set; }

    public enum GameState { Countdown, Playing, Paused }
    private GameState _currentState;

    // Events for other scripts to listen to (like UI, sound, etc.)
    public static event Action OnCountdownStarted;
    public static event Action OnCountdownFinished;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            // Use this if you load a separate game scene and want it to persist
            // DontDestroyOnLoad(gameObject);
        }
    }

    // Starts the countdown and runs the provided action when it's finished
    public void StartLevelCountdown(Action onCompleteCallback)
    {
        StartCoroutine(CountdownCoroutine(onCompleteCallback));
    }

    private IEnumerator CountdownCoroutine(Action onCompleteCallback)
    {
        Time.timeScale = 1f;
        _currentState = GameState.Countdown;
        OnCountdownStarted?.Invoke(); // Announce for UI

        // Wait for the UI to show 3, 2, 1, START!
        yield return new WaitForSeconds(4f);
        //Time.timeScale = 1f;
        // Run the code that was passed in (e.g., activate the level)
        onCompleteCallback?.Invoke();

        _currentState = GameState.Playing;
        OnCountdownFinished?.Invoke(); // Announce for player/timer
    }

    public bool IsPlaying()
    {
        return _currentState == GameState.Playing;
    }

    public void SetState(GameState newState)
    {
        _currentState = newState;
    }
}