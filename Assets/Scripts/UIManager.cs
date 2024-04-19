using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null) Debug.Log("AudioManager:NULL");
            return instance;
        }
    }
    private void Awake()
    {
        if(instance == null) instance = this;
    }

    [SerializeField] private TextMeshProUGUI _scoreTxt;
    void Start()
    {
        _scoreTxt.text = "";
    }
    private void Update()
    {
        DisplayScore();
    }
    private int _score;
    public void UpdateScore()
    {
        _score += 1;
    }
    private void DisplayScore()
    {
        _scoreTxt.text = _score.ToString();
    }



}
