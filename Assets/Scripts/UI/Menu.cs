using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button signInButton;
    [SerializeField] private Button applyReturnButton;

    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject optionsMenu;

    private void Start()
    {
        startMenu.SetActive(true);
        optionsMenu.SetActive(false);

        startButton.onClick.AddListener(StartGame);
        optionsButton.onClick.AddListener(ShowOptionsMenu);
        signInButton.onClick.AddListener(OpenSignInPage);
        applyReturnButton.onClick.AddListener(ShowStartMenu);
    }

    private void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void ShowOptionsMenu()
    {
        startMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    private void OpenSignInPage()
    {
        // Placeholder for sign-in logic - will print msg to console
        Debug.Log("Sign-in functionality goes here.");
        // Implement the sign-in functionality here?, possibly opening a new UI panel.
    }

    private void ShowStartMenu()
    {
        startMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }
}