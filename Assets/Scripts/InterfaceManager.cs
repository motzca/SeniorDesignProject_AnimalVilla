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
        if (gameManager == null)
        {
            Debug.LogError("GameManager is not assigned in InterfaceManager.");
            return;
        }

        if (gameManager.CurrentCard == null)
        {
            Debug.LogError("CurrentCard is null in GameManager.");
            return;
        }

        if (gameManager.Direction == "right")
        {
            UpdateImpactIcon(moneyStatusImpact, gameManager.CurrentCard.moneyStatRight);
            UpdateImpactIcon(energyStatusImpact, gameManager.CurrentCard.energyStatRight);
            UpdateImpactIcon(reputationStatusImpact, gameManager.CurrentCard.reputationStatRight);
        }
        else if (gameManager.Direction == "left")
        {
            UpdateImpactIcon(moneyStatusImpact, gameManager.CurrentCard.moneyStatLeft);
            UpdateImpactIcon(energyStatusImpact, gameManager.CurrentCard.energyStatLeft);
            UpdateImpactIcon(reputationStatusImpact, gameManager.CurrentCard.reputationStatLeft);
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
