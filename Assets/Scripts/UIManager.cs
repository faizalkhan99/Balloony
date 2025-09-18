using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null) Debug.Log("UIManager:NULL");
            return instance;
        }
    }

    [SerializeField] private GameObject _pauseMenuPanel;
    [SerializeField] private GameObject _mainmenuPanel;
    [SerializeField] private GameObject _gameModeSelectPanel;
    [SerializeField] private GameObject _restartPanel;
    [SerializeField] private GameObject _nextLevelPanel;
    [SerializeField] public GameObject _pauseButttonPanel;
    [SerializeField] private GameObject _creditsPanel;
    [SerializeField] private GameObject _loadingScreen;

    [SerializeField] private TextMeshProUGUI _scoreTxt;
    [SerializeField] private TextMeshProUGUI _scoreTxt_copy;
    [SerializeField] private Slider _progressBar;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        _isTouchWorking = true;
        TurnEverythingOFF();
        if (_mainmenuPanel) _mainmenuPanel.SetActive(true);
        if (_pauseButttonPanel) _pauseButttonPanel.SetActive(true);
        Time.timeScale = 1.0f;
        if (_scoreTxt)
        {
            _scoreTxt.text = "";
        }
    }
    private void Update()
    {
        DisplayScore();
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1.0f;
        TurnEverythingOFF();
        if (_loadingScreen) _loadingScreen.SetActive(true);
        StartCoroutine(LoadingAsync(sceneName));
    }
    // IEnumerator LoadingAsync(string sceneName)
    // {
    //     AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
    //     while (!operation.isDone)
    //     {
    //         _progressBar.value = operation.progress;
    //         yield return new WaitForEndOfFrame();
    //     }
    // }
    [SerializeField] private GameObject TransObject;
    [SerializeField] private float _transitionTime = 1f;
    IEnumerator LoadingAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        TransObject.SetActive(true);
        yield return new WaitForSeconds(_transitionTime);
        operation.allowSceneActivation = true;
        //SceneManager.LoadScene(sceneName);
    }



    // IEnumerator LoadingAsync(string sceneName)
    // {
    //     // --- Step 1: Start loading the scene but don't activate it yet ---
    //     AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
    //     operation.allowSceneActivation = false; // This is the key!

    //     // Set a minimum load time to ensure the screen is visible
    //     float minLoadTime = 3.0f;
    //     float elapsedTime = 0f;

    //     // --- Step 2: Simulate a smooth progress bar ---
    //     // We'll animate the slider's value while the real loading happens.
    //     // The loop continues until the real load is almost done AND our minimum time has passed.
    //     while (elapsedTime < minLoadTime || operation.progress < 0.9f)
    //     {
    //         elapsedTime += Time.deltaTime;

    //         // Calculate a simulated progress value that increases over our minimum load time
    //         float simulatedProgress = Mathf.Clamp01(elapsedTime / minLoadTime);

    //         // Use the HIGHER value between the real progress and our simulated progress
    //         // This ensures the bar never goes backward if real loading is faster.
    //         float displayProgress = Mathf.Max(operation.progress, simulatedProgress);

    //         // Smoothly move the progress bar towards the target value instead of jumping instantly
    //         _progressBar.value = Mathf.MoveTowards(_progressBar.value, displayProgress, Time.deltaTime);

    //         yield return null; // Wait for the next frame
    //     }

    //     // --- Step 3: Finish the loading bar and activate the scene ---
    //     // Animate the last 10% of the bar to make it feel complete.
    //     while (_progressBar.value < 1f)
    //     {
    //         _progressBar.value = Mathf.MoveTowards(_progressBar.value, 1f, Time.deltaTime * 2f); // Move a bit faster
    //         yield return null;
    //     }

    //     // Finally, allow the new scene to activate.
    //     operation.allowSceneActivation = true;
    // }




    private int _score;
    public void UpdateScore()
    {
        _score += 1;

        if (_score > 0 && _score % 10 == 0)
        {
            AudioManager.Instance.PlaySFX(SoundID.yay);
        }
    }
    public int ReturnScore()
    {
        return _score;
    }
    private void DisplayScore()
    {
        if (_scoreTxt)
        {
            _scoreTxt.text = _score.ToString();
            _scoreTxt_copy.text = _score.ToString();
        }
    }

    public bool _isTouchWorking;
    public void GamePauseUnpause(bool condition)
    {
        if (condition)
        {
            Time.timeScale = 0f;
            _isTouchWorking = false;
            AudioManager.Instance.PauseBGM();
            if (_pauseMenuPanel) _pauseMenuPanel.SetActive(condition); //true
            if (_pauseButttonPanel) _pauseButttonPanel.SetActive(!condition);  //!true
        }
        else
        {
            Time.timeScale = 1f;
            _isTouchWorking = true;
            AudioManager.Instance.UnpauseBGM();
            if (_pauseMenuPanel) _pauseMenuPanel.SetActive(condition); //false
            if (_pauseButttonPanel) _pauseButttonPanel.SetActive(!condition);  //!false
        }
    }

    [SerializeField] private Image _obstacleLoseImage;
    [SerializeField] private Image _balloonLoseImage;
    [SerializeField] private TextMeshProUGUI _genericLoseScreenText;
    public void GameOverScreen(string _loseReason)
    {
        Time.timeScale = 0f;
        _isTouchWorking = false;
        AudioManager.Instance.PauseBGM();
        TurnEverythingOFF();
        if (_restartPanel) _restartPanel.SetActive(true);
        if (_loseReason == "spikes" && _obstacleLoseImage)
        {
            _obstacleLoseImage.gameObject.SetActive(true);
            _balloonLoseImage?.gameObject.SetActive(false);
            _genericLoseScreenText?.gameObject.SetActive(false);
        }
        else if (_loseReason == "balloon" && _balloonLoseImage)
        {
            _balloonLoseImage.gameObject.SetActive(true);
            _obstacleLoseImage?.gameObject.SetActive(false);
            _genericLoseScreenText?.gameObject.SetActive(false);
        }
        else if (_loseReason == "generic" && _genericLoseScreenText)
        {
            _genericLoseScreenText?.gameObject.SetActive(true);
            _balloonLoseImage?.gameObject.SetActive(false);
            _obstacleLoseImage?.gameObject.SetActive(false);
        }
    }

    public void ShowNextLevelScreen()
    {
        Time.timeScale = 0f;
        AudioManager.Instance.PauseBGM();
        AudioManager.Instance.PlaySFX(SoundID.LevelComplete);
        TurnEverythingOFF();
        if (_nextLevelPanel) _nextLevelPanel.SetActive(true);
    }

    [SerializeField] private GameObject _winScreenPanel;
    public void WinScreen()
    {
        Time.timeScale = 0f;
        _isTouchWorking = false;
        TurnEverythingOFF();
        AudioManager.Instance.PauseBGM();
        AudioManager.Instance.PlaySFX(SoundID.GameWin);
        if (_winScreenPanel) _winScreenPanel.SetActive(true);
    }
    public void CreditsScreen()
    {
        TurnEverythingOFF();
        if (_creditsPanel) _creditsPanel.SetActive(true);
    }
    public void BackToHome()
    {
        TurnEverythingOFF();
        if (_mainmenuPanel) _mainmenuPanel.SetActive(true);
    }
    public void GameModeSelect()
    {
        TurnEverythingOFF();
        _gameModeSelectPanel?.gameObject.SetActive(true);
    }

    public void TurnEverythingOFF()
    {
        if (_creditsPanel) _creditsPanel.SetActive(false);
        if (_mainmenuPanel) _mainmenuPanel.SetActive(false);
        if (_restartPanel) _restartPanel.SetActive(false);
        if (_pauseMenuPanel) _pauseMenuPanel.SetActive(false);
        if (_loadingScreen) _loadingScreen.SetActive(false);
        if (_pauseButttonPanel) _pauseButttonPanel.SetActive(false);
        if (_gameModeSelectPanel) _gameModeSelectPanel.SetActive(false);
        if (_nextLevelPanel) _nextLevelPanel.SetActive(false);
        //if (_countDownTxt) _countDownTxt.gameObject.SetActive(false);
    }

    public void PlayButtonClickSFX()
    {
        AudioManager.Instance.PlaySFX(SoundID.ButtonClick);
    }
    public void ExternalLinks(string url)
    {
        Application.OpenURL(url);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}