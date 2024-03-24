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

        musicSource.loop = loopMusic;
        creditsMusicSource.loop = false;

        LoadAndApplySavedAudioSettings();

        if (playMusicOnAwake)
        {
            musicSource.Play();
            PlayAmbientSoundsIfAvailable();
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!nonPlayingScenes.Contains(scene.name))
        {
            if (!musicSource.isPlaying && isMusicActive)
            {
                PlayMusicIfAvailable();
            }
            PlayAllAmbientSounds();
        }
        else
        {
            StopMusicAndAmbientSounds();
        }

        if (scene.name == "Start")
        {
            EnsureMusicPlays();
        }
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

    private void PlayMusicIfAvailable()
    {
        if (musicSource.clip != null && isMusicActive)
        {
            musicSource.Play();
        }
    }

    private void PlayAmbientSoundsIfAvailable()
    {
        foreach (var clip in ambientClips)
        {
            PlayAmbientSound(clip, true);
        }
    }

    public void EnsureMusicPlays()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    private void PlayAllAmbientSounds()
    {
        foreach (var source in ambientSources)
        {
            if (!source.isPlaying)
            {
                source.Play();
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
        musicSource.Pause();
        PauseAmbientSounds();

        if (creditsMusicSource != null && creditsMusicSource.gameObject.activeInHierarchy && creditsMusicSource.enabled)
        {
            creditsMusicSource.Play();
        }
    }

    public void ResumeAfterCredits()
    {
        if (SceneManager.GetActiveScene().name == "Start")
        {
            musicSource.Stop();
            EnsureMusicPlays();
        }
        else
        {
            if (!musicSource.isPlaying && isMusicActive)
            {
                musicSource.Play();
            }
        }

        if (creditsMusicSource != null && creditsMusicSource.isPlaying)
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

    private void StopMusicAndAmbientSounds()
    {
        musicSource.Stop();
        StopAllAmbientSounds();
        isMusicActive = false;
    }

    public void ToggleActiveState()
    {
        isMusicActive = !isMusicActive;
        if (isMusicActive)
        {
            EnsureMusicPlays();
            foreach (var source in ambientSources)
            {
                if (!source.isPlaying)
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