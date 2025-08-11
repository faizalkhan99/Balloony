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
    AudioManager.Instance.PlaySFX(SoundID.CountdownTick); // Call by ID
    yield return new WaitForSecondsRealtime(1f);

    countdownText.text = "2";
    AudioManager.Instance.PlaySFX(SoundID.CountdownTick); // Call by ID
    yield return new WaitForSecondsRealtime(1f);

    countdownText.text = "1";
    AudioManager.Instance.PlaySFX(SoundID.CountdownTick); // Call by ID
    yield return new WaitForSecondsRealtime(1f);

    countdownText.text = "GO!";
    AudioManager.Instance.PlaySFX(SoundID.CountdownGo);   // Call by ID
    yield return new WaitForSecondsRealtime(1f);

    countdownText.gameObject.SetActive(false);
    }
}