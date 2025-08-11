using UnityEngine;
using TMPro;
using System.Collections;

public class CountdownUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private AudioClip _countdown321SFX;
    [SerializeField] private AudioClip _countdownGOSFX;


    void OnEnable()
    {
        GameCountdownManager.OnCountdownStarted += StartCountdownVisuals;
    }

    void OnDisable()
    {
        GameCountdownManager.OnCountdownStarted -= StartCountdownVisuals;
    }

    private void StartCountdownVisuals()
    {
        countdownText.gameObject.SetActive(true);
        StartCoroutine(CountdownSequence());
    }

    private IEnumerator CountdownSequence()
    {
        countdownText.text = "3";
        AudioManager.Instance.PlaySFX(_countdown321SFX);
        yield return new WaitForSecondsRealtime(1f); // Use REAL time

        countdownText.text = "2";
        AudioManager.Instance.PlaySFX(_countdown321SFX);
        yield return new WaitForSecondsRealtime(1f); // Use REAL time

        countdownText.text = "1";
        AudioManager.Instance.PlaySFX(_countdown321SFX);
        yield return new WaitForSecondsRealtime(1f); // Use REAL time

        countdownText.text = "GO!";
        AudioManager.Instance.PlaySFX(_countdownGOSFX);
        yield return new WaitForSecondsRealtime(1f); // Use REAL time

        countdownText.gameObject.SetActive(false);
    }
}