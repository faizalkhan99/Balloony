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
    [SerializeField] private float _surviveTime;
    private float _tempSurviveTime;

    void Start()
    {
        _tempSurviveTime = _surviveTime;
        _levels[_currentLevelIndex]?.SetActive(true);
    }
    void Update()
    {
        _levelNumberTxt.text = "Level: " + (_currentLevelIndex+1).ToString();
        _levelNumberTxt_copy.text = "Level: " + (_currentLevelIndex+1).ToString();
        
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