using UnityEngine;
using System.Collections.Generic;

public class CustomAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip musicClip;
    [SerializeField] private bool playMusicOnAwake = true;
    [SerializeField] private bool loopMusic = true;
    [SerializeField] private AudioClip[] ambientClips;
    [SerializeField] private bool playAmbientOnAwake = true;
    [SerializeField] private bool loopAmbient = true;

    private List<AudioSource> ambientSources = new List<AudioSource>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        LoadAndApplySavedAudioSettings();
        audioSource.loop = loopMusic;
        if (playMusicOnAwake && musicClip != null)
        {
            audioSource.clip = musicClip;
            audioSource.Play();
        }

        if (playAmbientOnAwake)
        {
            foreach (var clip in ambientClips)
            {
                PlayAmbientSound(clip, loopAmbient);
            }
        }
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.Play();
    }

    public void PlayAmbientSound(AudioClip clip, bool loop)
    {
        var ambientSource = gameObject.AddComponent<AudioSource>();
        ambientSource.clip = clip;
        ambientSource.loop = loop;
        ambientSource.volume = Mathf.Clamp(PlayerPrefs.GetFloat("AmbientVolume", 0.1f), 0f, 0.1f);
        ambientSource.Play();
        ambientSources.Add(ambientSource);
    }

    public void AdjustMusicVolume(float volume)
    {
        audioSource.volume = volume;
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
        AdjustMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 0.5f));
        float ambientVolume = Mathf.Clamp(PlayerPrefs.GetFloat("AmbientVolume", 0.1f), 0f, 0.1f);
        AdjustAmbientVolume(ambientVolume);
    }
}
