using UnityEngine;
using System;
using System.Collections;

public class GameCountdownManager : MonoBehaviour
{
    public static GameCountdownManager Instance { get; private set; }

    public enum GameState { Countdown, Playing, Paused }
    private GameState _currentState;

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
        }
    }

    public void StartLevelCountdown(Action onCompleteCallback)
    {
        StartCoroutine(CountdownCoroutine(onCompleteCallback));
    }

    private IEnumerator CountdownCoroutine(Action onCompleteCallback)
    {
        _currentState = GameState.Countdown;
        OnCountdownStarted?.Invoke(); // Tell the UI to show "3, 2, 1..."

        yield return new WaitForSecondsRealtime(4f);

        Time.timeScale = 1f;

        onCompleteCallback?.Invoke();

        _currentState = GameState.Playing;
        OnCountdownFinished?.Invoke();
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