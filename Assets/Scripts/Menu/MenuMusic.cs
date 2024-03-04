using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MenuMusic : MonoBehaviour
{
    private static bool isMusicActive = true;
    public List<string> nonPlayingScenes = new List<string>();
    private AudioSource audioSource;

    private void Awake()
    {
        GameObject[] musicObj = GameObject.FindGameObjectsWithTag("GameMusic");
        if (musicObj.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            LoadAndApplySavedVolumeSettings();
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void LoadAndApplySavedVolumeSettings()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        AdjustVolume(musicVolume);
    }

    public void AdjustVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (nonPlayingScenes.Contains(scene.name))
        {
            gameObject.SetActive(false);
            isMusicActive = false;
        }
        else
        {
            gameObject.SetActive(true);
            isMusicActive = true;
        }
    }

    public void ToggleActiveState()
    {
        isMusicActive = !isMusicActive;
        gameObject.SetActive(isMusicActive);
    }
}
