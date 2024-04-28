using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _levels;
    [SerializeField] private int _currentLevelIndex = 0;
    void Start()
    {
        _levels[_currentLevelIndex].SetActive(true);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextLevel();
        }
    }

    private void NextLevel()
    {
        _currentLevelIndex++;
        _levels[_currentLevelIndex].SetActive(true);
        _levels[_currentLevelIndex-1].SetActive(false);

    }
}
