using UnityEngine;
using Fungus;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    public Flowchart flowchart;
    private bool isEndingHandled = false;

    void OnEnable() {
        GameManager.OnMoneyZero += HandleMoneyZero;
        GameManager.OnEnergyZero += HandleEnergyZero;
        GameManager.OnReputationZero += HandleReputationZero;
    }

    void OnDisable() {
        GameManager.OnMoneyZero -= HandleMoneyZero;
        GameManager.OnEnergyZero -= HandleEnergyZero;
        GameManager.OnReputationZero -= HandleReputationZero;
    }

    private void HandleMoneyZero() {
         if (!isEndingHandled)
        {
            bool restartGame = flowchart.GetBooleanVariable("RestartGame");
            if (restartGame)
            {
                RestartGame();
            }
            else
            {
                flowchart.ExecuteBlock("Bad Ending (Money)");
                isEndingHandled = true;
            }
        }
    }

    private void HandleEnergyZero() {
        if (!isEndingHandled)
        {
            bool restartGame = flowchart.GetBooleanVariable("RestartGame");
            if (restartGame)
            {
                RestartGame();
            }
            else
            {
                flowchart.ExecuteBlock("Bad Ending (Energy)");
                isEndingHandled = true;
            }
        }
    }

    private void HandleReputationZero() {
         if (!isEndingHandled)
        {
            bool restartGame = flowchart.GetBooleanVariable("RestartGame");
            if (restartGame)
            {
                RestartGame();
            }
            else
            {
                flowchart.ExecuteBlock("Bad Ending (Reputation)");
                isEndingHandled = true;
            }
        }
    }

    public void RestartGame()
    {
        Debug.Log("RestartGame method called");
        isEndingHandled = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        isEndingHandled = false;
        SceneManager.LoadScene("MainMenu");
    }
}
