using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private Animator _transition;
    [SerializeField] private float _transitionTime = 1f;
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadLevel(sceneName));
    }

    IEnumerator LoadLevel(string levelName)
    {
        _transition.SetTrigger("start");
        yield return new WaitForSeconds(_transitionTime);
        SceneManager.LoadScene(levelName);
    }
}
