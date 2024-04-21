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
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        PlayBGM();
    }
    [SerializeField] AudioSource _bgm;
    [SerializeField] AudioSource[] _SFXAudioSources;


    public void PlayBGM()
    {
        if(_bgm) _bgm.PlayDelayed(0.3f);
    }
    public void PauseBGM()
    {
        if (_bgm) _bgm.Pause();
    }
    public void UnpauseBGM()
    {
        if (_bgm) _bgm.UnPause();
    }

    public void PlaySFX(AudioClip audio/*, float vol*/)
    {
        for (int i = 0; i < _SFXAudioSources.Length; i++)
        {
            if (!_SFXAudioSources[i].isPlaying)
            {
                _SFXAudioSources[i].clip = audio;
                //_SFXAudioSources[i].volume = vol;
                _SFXAudioSources[i].Play();
                break;
            }
        }
    }

}
