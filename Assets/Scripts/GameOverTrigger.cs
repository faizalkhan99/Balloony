using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Balloon"))
        {
            UIManager.Instance.GameOverScreen("balloon"); //2=balloon was the reason for player death.
        }
    }
}
