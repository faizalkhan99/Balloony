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
    [SerializeField] private GameObject _restartPanel;
    [SerializeField] private GameObject _pauseButttonPanel;
    [SerializeField] private GameObject _creditsPanel;
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private Slider _progressBar;
    private void Awake()
    {
        if(instance == null) instance = this;
    }

    [SerializeField] private TextMeshProUGUI _scoreTxt;
    void Start()
    {
        _isTouchWorking = true;
        TurnEverythingOFF();
        if(_mainmenuPanel) _mainmenuPanel.SetActive(true);
        if(_pauseButttonPanel) _pauseButttonPanel.SetActive(true);
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
        if(_loadingScreen) _loadingScreen.SetActive(true);
        StartCoroutine(LoadingAsync(sceneName));
    }
    IEnumerator LoadingAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            _progressBar.value = operation.progress;
            yield return new WaitForEndOfFrame();
        }
    }

    private int _score;
    public void UpdateScore()
    {
        _score += 1;
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
        }
    }

    public bool _isTouchWorking;
    public void GamePauseUnpause( bool condition)
    {
        if(condition)
        { 
            Time.timeScale = 0f;
            _isTouchWorking = false;
            AudioManager.Instance.PauseBGM();
            if(_pauseMenuPanel) _pauseMenuPanel.SetActive(condition); //true
            if(_pauseButttonPanel) _pauseButttonPanel.SetActive(!condition);  //!true
        }
        else
        {
            Time.timeScale = 1f;
            _isTouchWorking = true;
            AudioManager.Instance.UnpauseBGM();
            if(_pauseMenuPanel) _pauseMenuPanel.SetActive(condition); //false
            if(_pauseButttonPanel) _pauseButttonPanel.SetActive(!condition);  //!false
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
        if(_loseReason == "spikes" && _obstacleLoseImage)
        {
            _obstacleLoseImage.gameObject.SetActive(true);
            _balloonLoseImage?.gameObject.SetActive(false);
            _genericLoseScreenText?.gameObject.SetActive(false);
        }
        else if(_loseReason == "balloon" && _balloonLoseImage)
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
    [SerializeField] private GameObject _winScreenPanel;
    public void WinScreen()
    {
        Time.timeScale = 0f;
        _isTouchWorking = false;
        AudioManager.Instance.PauseBGM();
        TurnEverythingOFF();
        _winScreenPanel?.gameObject.SetActive(false);
    }
    public void CreditsScreen()
    {
        TurnEverythingOFF();
        if(_creditsPanel) _creditsPanel.SetActive(true);
    }
    public void BackToHome()
    {
        TurnEverythingOFF();
        if(_mainmenuPanel) _mainmenuPanel.SetActive(true); 
    }

    public void TurnEverythingOFF()
    {
        if(_creditsPanel) _creditsPanel.gameObject.SetActive(false);
        if (_mainmenuPanel) _mainmenuPanel.SetActive(false);
        if (_restartPanel) _restartPanel.SetActive(false);
        if(_pauseMenuPanel) _pauseMenuPanel.SetActive(false);
        if(_loadingScreen) _loadingScreen.SetActive(false);
        if(_pauseButttonPanel) _pauseButttonPanel.SetActive(false);
    }

    [SerializeField] private AudioClip _buttonClickIn;
    [SerializeField] private AudioClip _buttonClickOut;
    public void PlayButtonClickSFX(bool isThisInSound)
    {
        if(isThisInSound)
        {
            AudioManager.Instance.PlaySFX(_buttonClickIn);
        }
        else
        {
            AudioManager.Instance.PlaySFX(_buttonClickOut);
        }
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
