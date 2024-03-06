using UnityEngine;

public class EndingManager : MonoBehaviour
{
    public delegate void StatReachedZero(int cardId);
    public static event StatReachedZero OnMoneyZero;
    public static event StatReachedZero OnEnergyZero;
    public static event StatReachedZero OnReputationZero;

    private int endingMoneyCardId;
    private int endingEnergyCardId;
    private int endingReputationCardId;

    public void SetEndingCardIds(int moneyCardId, int energyCardId, int reputationCardId)
    {
        endingMoneyCardId = moneyCardId;
        endingEnergyCardId = energyCardId;
        endingReputationCardId = reputationCardId;
    }

    public void CheckStatus()
    {
        if (GameManager.MoneyStatus <= 0 && OnMoneyZero != null)
        {
            OnMoneyZero(endingMoneyCardId);
        }
        else if (GameManager.EnergyStatus <= 0 && OnEnergyZero != null)
        {
            OnEnergyZero(endingEnergyCardId);
        }
        else if (GameManager.ReputationStatus <= 0 && OnReputationZero != null)
        {
            OnReputationZero(endingReputationCardId);
        }
    }
}
