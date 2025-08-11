using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Objects")]
    [SerializeField] private GameObject[] _levels;
    private int _currentLevelIndex = 0; // Starts at 0, representing the first level

    [Header("Level UI")]
    [SerializeField] private TextMeshProUGUI _levelNumberTxt;
    [SerializeField] private TextMeshProUGUI _levelNumberTxt_copy;
    [SerializeField] private TextMeshProUGUI _timeToSurviveText;
    [SerializeField] private TextMeshProUGUI _timeToSurviveText_copy;

    [Header("Survival Timer")]
    [SerializeField] private float[] _surviveTimeRange;
    private float _surviveTimeRemaining;

    void Start()
    {
        foreach (var level in _levels)
        {
            level.SetActive(false);
        }
        // Start the game by preparing Level 0
        PrepareAndStartNextLevel();
    }

void Update()
{
    // Check if we are in the "Playing" state
    if (GameCountdownManager.Instance != null && GameCountdownManager.Instance.IsPlaying())
    {
        // Check if the timer has run out
        if (_surviveTimeRemaining <= 0)
        {
            // --- IMPROVEMENT ---
            // Set the state to Paused FIRST to prevent this block from running again.
            GameCountdownManager.Instance.SetState(GameCountdownManager.GameState.Paused);
            ShowLevelCompleteScreen();
        }
        else
        {
            _surviveTimeRemaining -= Time.deltaTime;
            _timeToSurviveText.text = _surviveTimeRemaining.ToString("F0");
            _timeToSurviveText_copy.text = _surviveTimeRemaining.ToString("F0");
        }
    }
}

    public void PrepareAndStartNextLevel()
    {
        if (UIManager.Instance != null) UIManager.Instance.TurnEverythingOFF();

        // First, check if we've finished all levels.
        if (_currentLevelIndex >= _levels.Length)
        {
            UIManager.Instance.WinScreen();
            return;
        }

        // --- THE CORE FIX IS HERE ---
        // We create the action that will run AFTER the countdown.
        System.Action startLevelAction = () =>
        {
            // FIX #3: Deactivate the PREVIOUS level first.
            // This runs AFTER the countdown, right before the new level starts.
            if (_currentLevelIndex > 0)
            {
                _levels[_currentLevelIndex - 1]?.SetActive(false);
            }

            // Now, activate the CURRENT(jo next ana chahiye) level.
            _levels[_currentLevelIndex].SetActive(true);
            EnemyList.Instance.DeleteAllClones();
            UIManager.Instance.GamePauseUnpause(false);
            // Reset its timer and update its UI text.
            ResetTimerForNewLevel();
            UpdateLevelText();

            // FIX #2: Increment the index for the NEXT cycle only AFTER this level is fully set up.
            _currentLevelIndex++;
        };

        // Now, we tell the manager to start the countdown and run our action when done.
        GameCountdownManager.Instance.StartLevelCountdown(startLevelAction);
    }

    private void ShowLevelCompleteScreen()
    {
        UIManager.Instance.ShowNextLevelScreen();
        AudioManager.Instance.PauseBGM();
    }

    private void ResetTimerForNewLevel()
    {
        int rand = Random.Range(0, _surviveTimeRange.Length);
        _surviveTimeRemaining = _surviveTimeRange[rand];
    }

    private void UpdateLevelText()
    {
        // We add 1 to the index for display because arrays start at 0 (Level 0 is "Level 1").
        _levelNumberTxt.text = "Level " + (_currentLevelIndex + 1).ToString();
        _levelNumberTxt_copy.text = "Level " + (_currentLevelIndex + 1).ToString();
    }
}


/* //My Old Script
using TMPro;
using UnityEngine;
public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _levels;
    [SerializeField] private int _currentLevelIndex = 0;
    [SerializeField] private TextMeshProUGUI _levelNumberTxt;
    [SerializeField] private TextMeshProUGUI _levelNumberTxt_copy;
    [SerializeField] private TextMeshProUGUI _timeToSurviveText;
    [SerializeField] private TextMeshProUGUI _timeToSurviveText_copy;
    [SerializeField] private float[] _surviveTimeRange;
    [SerializeField] private float _surviveTime;
    private float _tempSurviveTime;

    private void OnEnable()
    {
        int rand = Random.Range(0, _surviveTimeRange.Length);
        _surviveTime = _surviveTimeRange[rand];
    }

    void Start()
    {
        _tempSurviveTime = _surviveTime;
        _levels[_currentLevelIndex]?.SetActive(true);
    }
    void Update()
    {
        _levelNumberTxt.text = "Level: " + (_currentLevelIndex + 1).ToString();
        _levelNumberTxt_copy.text = "Level: " + (_currentLevelIndex + 1).ToString();

        if (_surviveTime <= 0)
        {
            NextLevel();
        }
        else
        {
            _surviveTime -= Time.deltaTime;
            _timeToSurviveText.text = _surviveTime.ToString("F0");
            _timeToSurviveText_copy.text = _surviveTime.ToString("F0");
        }
    }
    private void NextLevel()
    {
        _levels[_currentLevelIndex]?.SetActive(false);
        EnemyList.Instance.DeleteAllClones();

        // Check if there are more levels
        if (_currentLevelIndex + 1 < _levels.Length)
        {
            _currentLevelIndex++;
            _levels[_currentLevelIndex]?.SetActive(true);
            _surviveTime = _tempSurviveTime;
        }
        else
        {
            UIManager.Instance.WinScreen();
        }
    }
}
*/