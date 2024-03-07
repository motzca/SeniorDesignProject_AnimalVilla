using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip[] ambientClips;
    [SerializeField] private bool playMusicOnAwake = true;
    [SerializeField] private bool loopMusic = true;
    [SerializeField] private List<string> nonPlayingScenes = new List<string>();

    private List<AudioSource> ambientSources = new List<AudioSource>();
    private static bool isMusicActive = true;

    private static AudioManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource.loop = loopMusic;
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
        LoadAndApplySavedAudioSettings();
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
        ambientSource.volume = Mathf.Clamp(PlayerPrefs.GetFloat("AmbientVolume", 0.1f), 0f, 0.1f);
        ambientSource.Play();
        ambientSources.Add(ambientSource);
    }

    public void AdjustMusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    public void AdjustAmbientVolume(float volume)
    {
        float adjustedVolume = Mathf.Clamp(volume, 0f, 0.1f);
        
        foreach (var source in ambientSources)
        {
            source.volume = adjustedVolume;
        }
        PlayerPrefs.SetFloat("AmbientVolume", adjustedVolume);
        PlayerPrefs.Save();
    }

    private void LoadAndApplySavedAudioSettings()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        AdjustMusicVolume(musicVolume);
        float ambientVolume = Mathf.Clamp(PlayerPrefs.GetFloat("AmbientVolume", 0.1f), 0f, 0.1f);
        AdjustAmbientVolume(ambientVolume);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (nonPlayingScenes.Contains(scene.name))
        {
            musicSource.Stop();
            isMusicActive = false;
        }
        else
        {
            if (!musicSource.isPlaying)
            {
                musicSource.Play();
            }
            isMusicActive = true;
        }
    }

    public void ToggleActiveState()
    {
        isMusicActive = !isMusicActive;
        musicSource.gameObject.SetActive(isMusicActive);
    }
}
