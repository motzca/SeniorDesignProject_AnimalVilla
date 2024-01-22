using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject card;

    public Image moneyStatus;
    public Image energyStatus;
    public Image reputationStatus;

    public Image moneyStatusImpact;
    public Image energyStatusImpact;
    public Image reputationStatusImpact;

    void Update()
    {
        moneyStatus.fillAmount = (float)GameManager.MoneyStatus / GameManager.MaxValue;
        energyStatus.fillAmount = (float)GameManager.EnergyStatus / GameManager.MaxValue;
        reputationStatus.fillAmount = (float)GameManager.ReputationStatus / GameManager.MaxValue;

        UpdateImpactIcons();
    }

    private void UpdateImpactIcons()
    {
        if(gameManager.direction == "right")
        {
            UpdateImpactIcon(moneyStatusImpact, gameManager.currentCard.moneyStatRight);
            UpdateImpactIcon(energyStatusImpact, gameManager.currentCard.energyStatRight);
            UpdateImpactIcon(reputationStatusImpact, gameManager.currentCard.reputationStatRight);
        }
        else if (gameManager.direction == "left")
        {
            UpdateImpactIcon(moneyStatusImpact, gameManager.currentCard.moneyStatLeft);
            UpdateImpactIcon(energyStatusImpact, gameManager.currentCard.energyStatLeft);
            UpdateImpactIcon(reputationStatusImpact, gameManager.currentCard.reputationStatLeft);
        }
        else
        {
            ResetImpactIcons();
        }
    }

    private void UpdateImpactIcon(Image impactIcon, int statChange)
    {
        impactIcon.transform.localScale = statChange != 0 ? new Vector3(1, 1, 0) : Vector3.zero;
    }

    private void ResetImpactIcons()
    {
        moneyStatusImpact.transform.localScale = Vector3.zero;
        energyStatusImpact.transform.localScale = Vector3.zero;
        reputationStatusImpact.transform.localScale = Vector3.zero;
    }
}
