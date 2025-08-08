using System.Collections;
using UnityEngine;

public class Game2GameOver : MonoBehaviour
{
    [SerializeField] private bool _invisibleAfterDeath;
    [SerializeField] private AudioClip _game2GameOverSFX;
    [SerializeField] private AudioClip _balloonPopSFX;

    private Collider2D A, B;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Balloon") || collision.CompareTag("Obstacle"))
        {
            GameManager.Instance?.EndGame();
        }
        //if (A.bounds.Intersects(B.bounds)) if two boundaries touch each other.
        {
        
    }


    }

    IEnumerator DelayedCall()
    {
        yield return new WaitForSeconds(1.0f);
        if (_balloonPopSFX) AudioManager.Instance.PlaySFX(_balloonPopSFX);
            Debug.Log("awaaaz 2");

    }
}