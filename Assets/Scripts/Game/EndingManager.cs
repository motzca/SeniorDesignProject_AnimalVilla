using UnityEngine;

public class EndingManager : MonoBehaviour
{
    public void CheckForStatEndings(int moneyStatus, int energyStatus, int reputationStatus, int endingMoney, int endingEnergy, int endingReputation) {
        if (moneyStatus <= endingMoney) {
            LoadEndingCard(1); // This is subject to change once we add in the cards
        }
        else if (energyStatus <= endingEnergy) {
            LoadEndingCard(2); // This is subject to change once we add in the cards
        }
        else if (reputationStatus <= endingReputation){
            LoadEndingCard(3); // This is subject to change once we add in the cards
        }
    }

    private static void LoadEndingCard(int cardId) {
        GameManager.Instance.LoadStatEndingCard(cardId);
    }
}
