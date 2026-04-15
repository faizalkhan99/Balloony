using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BootstrapManager : MonoBehaviour
{
    [SerializeField] private int _waitTime;
    void Start()
    {
        StartCoroutine(WaitAndLoad());
    }
    
    private IEnumerator WaitAndLoad()
    {
        yield return new WaitForSeconds(_waitTime);
        SceneManager.LoadScene("MainMenu");
        BroAdsMan.ShowInterstitialAd(null);
    }
}