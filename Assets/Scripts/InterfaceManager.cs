using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    // Card
    public GameManager gameManager;
    public DialogManager dialogManager;

    // UI icons
    public Image moneyStatus;
    public Image energyStatus;
    public Image reputationStatus;

    // UI impact icons
    public Image moneyStatusImpact;
    public Image energyStatusImpact;
    public Image reputationStatusImpact;

    void Update()
    {
        // UI icons
        moneyStatus.fillAmount = (float)GameManager.moneyStatus / GameManager.maxValue;
        energyStatus.fillAmount = (float)GameManager.energyStatus / GameManager.maxValue;
        reputationStatus.fillAmount = (float)GameManager.reputationStatus / GameManager.maxValue;

        // UI impact
        DialogNode currentDialogNode = gameManager.dialogSystem.GetCurrentNode();

        if (currentDialogNode != null)
        {
            Card currentCard = currentDialogNode.card;

            // Right
            if (gameManager.direction == "right")
            {
                SetImpactIconScale(currentCard.moneyStatRight, moneyStatusImpact);
                SetImpactIconScale(currentCard.energyStatRight, energyStatusImpact);
                SetImpactIconScale(currentCard.reputationStatRight, reputationStatusImpact);
                dialogManager.StartDialog();
            }
            // Left
            else if (gameManager.direction == "left")
            {
                SetImpactIconScale(currentCard.moneyStatLeft, moneyStatusImpact);
                SetImpactIconScale(currentCard.energyStatLeft, energyStatusImpact);
                SetImpactIconScale(currentCard.reputationStatLeft, reputationStatusImpact);
                dialogManager.StartDialog();
            }
            else
            {
                ResetImpactIcons();
            }
        }
    }

    private void SetImpactIconScale(int statValue, Image icon)
    {
        icon.transform.localScale = (statValue != 0) ? new Vector3(1, 1, 0) : new Vector3(0, 0, 0);
    }

    private void ResetImpactIcons()
    {
        moneyStatusImpact.transform.localScale = new Vector3(0, 0, 0);
        energyStatusImpact.transform.localScale = new Vector3(0, 0, 0);
        reputationStatusImpact.transform.localScale = new Vector3(0, 0, 0);
    }
}

