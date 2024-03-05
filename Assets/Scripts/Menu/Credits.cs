using System.Collections;
using UnityEngine;

public class Credits : MonoBehaviour
{
    [SerializeField] private AudioSource creditsMusic;
    [SerializeField] private GameObject creditsMenu; 
    [SerializeField] private GameObject startMenu;

    void OnEnable()
    {
        creditsMusic.time = 0;
        creditsMusic.Play();
        creditsMenu.SetActive(true);
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            ReturnToMenu();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToMenu();
        }
    }

    private void ReturnToMenu()
    {
        creditsMusic.Pause();
        creditsMenu.SetActive(false);
        startMenu.SetActive(true);
    }
}

