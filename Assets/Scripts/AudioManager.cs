using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log("AudioManager:NULL");
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance)
        {
            instance = this;
        }
    }
    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
