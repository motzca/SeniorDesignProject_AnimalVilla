using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MenuMusic : MonoBehaviour
{
    private static bool isMusicActive = true;
    public List<string> nonPlayingScenes = new List<string>();

    private void Awake()
    {
        GameObject[] musicObj = GameObject.FindGameObjectsWithTag("GameMusic");
        if (musicObj.Length > 1)
        {
            Destroy(gameObject); 
        }
        else
        {
            DontDestroyOnLoad(gameObject); // Keeps music playing across scenes
        }

        SceneManager.sceneLoaded += OnSceneLoaded; 
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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

    public void AdjustVolume(float volume)
    {
        GetComponent<AudioSource>().volume = volume / 100f; // Assuming volume is a 0-100 value
    }

    public void ToggleActiveState()
    {
        isMusicActive = !isMusicActive;
        gameObject.SetActive(isMusicActive);
    }
}
