using System.Collections;
using UnityEngine;

public class Credits : MonoBehaviour
{
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private GameObject startMenu;
    [SerializeField] private RectTransform firstCreditsTextTransform;
    [SerializeField] private TypewriterEffect typewriterEffect;
    [SerializeField] private float scrollSpeed = 30f;
    [SerializeField] private float endPosition = 5000f;
    [SerializeField] private float startPosition = -1000f;

    void OnEnable()
    {
        AudioManager.Instance.PauseForCredits();
        creditsMenu.SetActive(true);
        ResetCreditsPosition();
        RestartEffects();
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

        if (creditsTextTransform.anchoredPosition.y >= endPosition)
        {
            ReturnToMenu();
        }
    }

    private void ResetCreditsPosition()
    {
        firstCreditsTextTransform.anchoredPosition = new Vector2(firstCreditsTextTransform.anchoredPosition.x, startPosition);
    }

    private void RestartEffects()
    {
        if (typewriterEffect != null)
        {
            typewriterEffect.RestartTypingEffect();
        }
    }

        private void ReturnToMenu()
    {
        AudioManager.Instance.ResumeAfterCredits();
        creditsMenu.SetActive(false);
        startMenu.SetActive(true);
    }
}
