using UnityEngine;
using TMPro;
using System.Collections;

public class CountdownUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

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
        Debug.Log("Countdown set to true!");
        StartCoroutine(CountdownSequence());
    }

    private IEnumerator CountdownSequence()
    {
        Debug.Log("Countdown started!");

        countdownText.text = "3";
        yield return new WaitForSeconds(1f);

        countdownText.text = "2";
        yield return new WaitForSeconds(1f);

        countdownText.text = "1";
        yield return new WaitForSeconds(1f);

        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);

        Debug.Log("Countdown ended!");

        countdownText.gameObject.SetActive(false);
        Debug.Log("Countdown set to false!");

    }
}