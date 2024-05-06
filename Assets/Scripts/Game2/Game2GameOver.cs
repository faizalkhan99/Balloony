using UnityEngine;

public class Game2GameOver : MonoBehaviour
{
    [SerializeField] private bool _invisibleAfterDeath;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Balloon") || collision.CompareTag("Obstacle"))
        {

            Time.timeScale = 0;
            UIManager.Instance.GameOverScreen("generic");
            if (_invisibleAfterDeath)
            {
                Destroy(this.gameObject);

            }
        }
    }
}