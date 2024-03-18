using System.Collections;
using UnityEngine;

public class Credits : MonoBehaviour
{
    [SerializeField] private AudioSource creditsMusic;
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private GameObject startMenu;
    [SerializeField] private RectTransform firstCreditsTextTransform; 
    [SerializeField] private float scrollSpeed = 30f;
    [SerializeField] private float resetPosition = 5000f;
    [SerializeField] private float startPosition = -1000f;

    void OnEnable()
    {
        creditsMusic.time = 0;
        creditsMusic.Play();
        creditsMenu.SetActive(true);
        ResetCreditsPosition();
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToMenu();
        }

        ScrollCredits(firstCreditsTextTransform);
    }

    private void ScrollCredits(RectTransform creditsTextTransform)
    {
        creditsTextTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);

        if (creditsTextTransform.anchoredPosition.y >= resetPosition)
        {
            creditsTextTransform.anchoredPosition = new Vector2(creditsTextTransform.anchoredPosition.x, startPosition);
        }
    }

    private void ResetCreditsPosition()
    {
        firstCreditsTextTransform.anchoredPosition = new Vector2(firstCreditsTextTransform.anchoredPosition.x, startPosition);
    }

    private void ReturnToMenu()
    {
        creditsMusic.Pause();
        creditsMenu.SetActive(false);
        startMenu.SetActive(true);
    }
}
