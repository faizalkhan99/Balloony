using UnityEngine;

public class Game2GameOver : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
            Time.timeScale = 0;
            UIManager.Instance.GameOverScreen("generic");
            Destroy(this.gameObject);
    }
}
