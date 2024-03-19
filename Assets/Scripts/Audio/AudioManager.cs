using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource creditsMusicSource;  
    [SerializeField] private AudioClip[] ambientClips;
    [SerializeField] private bool playMusicOnAwake = true;
    [SerializeField] private bool loopMusic = true;
    [SerializeField] private List<string> nonPlayingScenes = new List<string>();

    private List<AudioSource> ambientSources = new List<AudioSource>();
    private static bool isMusicActive = true;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadAndApplySavedAudioSettings();
        musicSource.loop = loopMusic;
        creditsMusicSource.loop = false; 
        if (playMusicOnAwake && musicSource.clip != null)
        {
            musicSource.Play();
        }
        if (ambientClips.Length > 0 && playMusicOnAwake)
        {
            foreach (var clip in ambientClips)
            {
                PlayAmbientSound(clip, true);
            }
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void PlayAmbientSound(AudioClip clip, bool loop)
    {
        AudioSource ambientSource = gameObject.AddComponent<AudioSource>();
        ambientSource.clip = clip;
        ambientSource.loop = loop;
        ambientSource.volume = Mathf.Clamp(PlayerPrefs.GetFloat("AmbientVolume", 0.1f), 0f, 1f);
        ambientSource.Play();
        ambientSources.Add(ambientSource);
    }

    public void AdjustMusicVolume(float volume)
    {
        musicSource.volume = volume;
        creditsMusicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    public void AdjustAmbientVolume(float volume)
    {
        foreach (var source in ambientSources)
        {
            source.volume = Mathf.Clamp(volume, 0f, 1f);
        }
        PlayerPrefs.SetFloat("AmbientVolume", volume);
        PlayerPrefs.Save();
    }

    private void LoadAndApplySavedAudioSettings()
    {
        AdjustMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 0.5f));
        AdjustAmbientVolume(PlayerPrefs.GetFloat("AmbientVolume", 0.1f));
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (nonPlayingScenes.Contains(scene.name))
        {
            musicSource.Stop();
            StopAllAmbientSounds();
            isMusicActive = false;
        }
        else
        {
            if (!musicSource.isPlaying && isMusicActive)
            {
                musicSource.Play();
            }
        }
    }

    private void StopAllAmbientSounds()
    {
        foreach (var source in ambientSources)
        {
            source.Stop();
        }
    }

    public void PauseAmbientSounds()
    {
        foreach (var source in ambientSources)
        {
            if (source.isPlaying)
            {
                source.Pause();
            }
        }
    }

    public void PauseForCredits()
    {
        if (musicSource.gameObject.activeInHierarchy && musicSource.enabled)
        {
            musicSource.Pause();
        }
        if (creditsMusicSource.gameObject.activeInHierarchy && creditsMusicSource.enabled)
        {
            creditsMusicSource.Play();
        }
        PauseAmbientSounds();
    }

    public void ResumeAfterCredits()
    {
        if (!musicSource.isPlaying && isMusicActive)
        {
            musicSource.UnPause();
        }
        if (creditsMusicSource.isPlaying)
        {
            creditsMusicSource.Stop();
        }
        ResumeAmbientSounds();
    }

    private void ResumeAmbientSounds()
    {
        foreach (var source in ambientSources)
        {
            if (!source.isPlaying)
            {
                source.UnPause();
            }
        }
    }

    public void ToggleActiveState()
    {
        isMusicActive = !isMusicActive;
        if (isMusicActive)
        {
            if (musicSource.gameObject.activeInHierarchy && musicSource.enabled)
            {
                musicSource.Play();
            }
            foreach (var source in ambientSources)
            {
                if (!source.isPlaying && source.gameObject.activeInHierarchy && source.enabled)
                {
                    source.Play();
                }
            }
        }
        else
        {
            musicSource.Pause();
            PauseAmbientSounds();
        }
    }
}
