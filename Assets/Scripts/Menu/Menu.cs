using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button exitButton; 
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject optionsMenu;

    private void Start()
    {
        startMenu.SetActive(true);
        optionsMenu.SetActive(false);

        startButton.onClick.AddListener(StartGame);
        optionsButton.onClick.AddListener(ShowOptionsMenu);
        exitButton.onClick.AddListener(ExitGame);
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

    private void ExitGame()
    {
        Application.Quit();
        // For Unity Editor (testing purposes)
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
