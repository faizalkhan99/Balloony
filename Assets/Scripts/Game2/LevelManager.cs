using TMPro;
using UnityEngine;
public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _levels;
    [SerializeField] private int _currentLevelIndex = 0;
    [SerializeField] private TextMeshProUGUI _levelNumberTxt;
    [SerializeField] private TextMeshProUGUI _timeToSurviveText;
    [SerializeField] private float _surviveTime;
    private float _tempSurviveTime;
    void Start()
    {
        _tempSurviveTime = _surviveTime;
        _levels[_currentLevelIndex].SetActive(true);
    }
    void Update()
    {
        _levelNumberTxt.text = "Lv " + (_currentLevelIndex+1).ToString();
        
        if (_surviveTime<=0)
        {
            NextLevel();
        }
        else
        {
            _surviveTime -= Time.deltaTime;
            _timeToSurviveText.text = _surviveTime.ToString("F0");
        }
    }
    private void NextLevel()
    {
        if (_currentLevelIndex < _levels.Length - 1)
        {
            _currentLevelIndex++;
            _levels[_currentLevelIndex]?.SetActive(true);
            _levels[_currentLevelIndex - 1]?.SetActive(false);
            _surviveTime = _tempSurviveTime;
        }
        else
        {
            UIManager.Instance.GameOverScreen("balloon"); //game finished screen here.
        }
    }
}