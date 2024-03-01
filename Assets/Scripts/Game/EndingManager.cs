using UnityEngine;

public class EndingManager : MonoBehaviour
{
    public void CheckForStatEndings(int moneyStatus, int energyStatus, int reputationStatus, int endingMoney, int endingEnergy, int endingReputation) {
        if (moneyStatus <= endingMoney) {
            LoadEndingCard("MoneyEnding");
        }
        else if (energyStatus <= endingEnergy) {
            LoadEndingCard("EnergyEnding");
        }
        else if (reputationStatus <= endingReputation){
            LoadEndingCard("ReputationEnding");
        }
    }

    private static void LoadEndingCard(string endingCardName) {
        GameManager.Instance.LoadStatEndingCard(endingCardName);
    }
}
