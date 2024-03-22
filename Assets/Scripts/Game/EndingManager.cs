using UnityEngine;
using Fungus;

public class EndingManager : MonoBehaviour
{
    public Flowchart day1Flowchart;

    private void OnEnable()
    {
        GameManager.OnMoneyZero += HandleMoneyZero;
        GameManager.OnEnergyZero += HandleEnergyZero;
        GameManager.OnReputationZero += HandleReputationZero;
    }

    private void OnDisable()
    {
        GameManager.OnMoneyZero -= HandleMoneyZero;
        GameManager.OnEnergyZero -= HandleEnergyZero;
        GameManager.OnReputationZero -= HandleReputationZero;
    }

    private void HandleMoneyZero(int cardId)
    {
        day1Flowchart.ExecuteBlock("Bad Ending (Money)");
        EndGame();
    }

    private void HandleEnergyZero(int cardId)
    {
        day1Flowchart.ExecuteBlock("Bad Ending (Energy)");
        EndGame();
    }

    private void HandleReputationZero(int cardId)
    {
        day1Flowchart.ExecuteBlock("Bad Ending (Reputation)");
        EndGame();
    }

    private void EndGame()
    {
        // Add logic to end the game and display a pop-up button to go back to the menu screen
    }
}
