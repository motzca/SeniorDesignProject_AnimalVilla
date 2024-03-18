using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject creditsMenu;

    private void Start()
    {
        startMenu.SetActive(true);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(false);

        startButton.onClick.AddListener(StartGame);
        optionsButton.onClick.AddListener(ShowOptionsMenu);
        exitButton.onClick.AddListener(ExitGame);
        creditsButton.onClick.AddListener(ShowCreditsMenu);
    }

    private void StartGame()
    {
        SceneManager.LoadScene("Day1");
    }

    private void ShowOptionsMenu()
    {
        startMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    private void ShowCreditsMenu()
    {
        startMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    private void ExitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
