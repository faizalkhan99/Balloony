using UnityEngine;

public class Game2GameOver : MonoBehaviour
{
    [SerializeField] private bool _invisibleAfterDeath;
    //private Collider2D A, B;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Balloon") || collision.CompareTag("Obstacle"))
        {
            GameManager.Instance.EndGame();
        }
        //if (A.bounds.Intersects(B.bounds)) if two boundaries touch each other.
    }
}