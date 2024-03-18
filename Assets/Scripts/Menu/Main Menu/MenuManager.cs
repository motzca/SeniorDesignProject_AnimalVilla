using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject optionsMenu;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OpenStartMenu()
    {
        startMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void OpenOptionsMenu()
    {
        startMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }
}
