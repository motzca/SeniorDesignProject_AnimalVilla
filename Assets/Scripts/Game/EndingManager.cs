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
            flowchart.ExecuteBlock("Bad Ending (Money)");
            isEndingHandled = true;
        }
    }

    private void HandleEnergyZero() {
        if (!isEndingHandled)
        {
            flowchart.ExecuteBlock("Bad Ending (Energy)");
            isEndingHandled = true;
        }
    }

    private void HandleReputationZero() {
         if (!isEndingHandled)
        {
            flowchart.ExecuteBlock("Bad Ending (Reputation)");
            isEndingHandled = true;
        }
    }

    public void RestartGame()
    {
        isEndingHandled = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        isEndingHandled = false;
        SceneManager.LoadScene("MainMenu");
    }
}
