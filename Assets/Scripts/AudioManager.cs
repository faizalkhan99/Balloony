using UnityEngine;
using System.Collections; // Required for using Coroutines
using System.Collections.Generic;

// This helper class lets us create an editable list in the Inspector.
[System.Serializable]
public class Sound
{
    public SoundID id;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    // --- Singleton Setup ---
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null) Debug.LogError("AudioManager is NULL.");
            return instance;
        }
    }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource _bgmSource;
    // This is your pool of pre-made sources for common sounds.
    [SerializeField] private AudioSource[] _sfxSources;

    [Header("Audio Clips Library")]
    [SerializeField] private Sound[] _sfxLibrary;

    private Dictionary<SoundID, AudioClip> _sfxDictionary;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        // Populate the Dictionary for fast lookups.
        _sfxDictionary = new Dictionary<SoundID, AudioClip>();
        foreach (var sound in _sfxLibrary)
        {
            _sfxDictionary[sound.id] = sound.clip;
        }
    }

    private void Start()
    {
        PlayBGM();
    }

    // --- BGM Controls ---
    public void PlayBGM() => _bgmSource?.PlayDelayed(0.3f);
    public void PauseBGM() => _bgmSource?.Pause();
    public void UnpauseBGM() => _bgmSource?.UnPause();

    // --- THE UPGRADED SFX METHOD ---
    public void PlaySFX(SoundID id)
    {
        if (!_sfxDictionary.ContainsKey(id))
        {
            Debug.LogWarning("AudioManager: Sound ID not found in library: " + id);
            return;
        }

        AudioClip clipToPlay = _sfxDictionary[id];

        // 1. First, try to use one of the pre-made audio sources.
        for (int i = 0; i < _sfxSources.Length; i++)
        {
            if (!_sfxSources[i].isPlaying)
            {
                _sfxSources[i].PlayOneShot(clipToPlay);
                return; // Sound played, job done.
            }
        }

        // 2. If all pre-made sources are busy, create a temporary one.
        StartCoroutine(CreateTemporarySourceAndPlay(clipToPlay));
    }

    private IEnumerator CreateTemporarySourceAndPlay(AudioClip clip)
    {
        // Create a new, temporary GameObject to host the AudioSource.
        GameObject tempGO = new GameObject("TempAudio_" + clip.name);
        tempGO.transform.SetParent(this.transform); // Keep the hierarchy clean.

        // Add and configure the AudioSource component.
        AudioSource tempSource = tempGO.AddComponent<AudioSource>();

        // Optional but recommended: Copy settings from your template sources.
        if (_sfxSources.Length > 0)
        {
            tempSource.outputAudioMixerGroup = _sfxSources[0].outputAudioMixerGroup;
            tempSource.spatialBlend = _sfxSources[0].spatialBlend;
            // Copy any other settings you want to keep consistent.
        }

        // Play the clip.
        tempSource.PlayOneShot(clip);

        // Wait for the length of the clip.
        yield return new WaitForSeconds(clip.length);

        // The sound has finished playing, now destroy the temporary GameObject.
        Destroy(tempGO);
    }
}