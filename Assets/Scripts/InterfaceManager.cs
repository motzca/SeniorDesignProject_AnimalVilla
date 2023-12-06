using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    // Card
    public GameManager gameManager;
    public GameObject card;

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
        moneyStatus.fillAmount = (float) GameManager.moneyStatus / GameManager.maxValue;
        energyStatus.fillAmount = (float) GameManager.energyStatus / GameManager.maxValue;
        reputationStatus.fillAmount = (float) GameManager.reputationStatus / GameManager.maxValue;

        // UI impact
        //Right
        if(gameManager.direction == "right")
        {
            if (gameManager.currentCard.moneyStatRight != 0)
                moneyStatusImpact.transform.localScale = new Vector3(1, 1, 0);
            if (gameManager.currentCard.energyStatRight != 0)
                energyStatusImpact.transform.localScale = new Vector3(1, 1, 0);
            if (gameManager.currentCard.reputationStatRight != 0)
                reputationStatusImpact.transform.localScale = new Vector3(1, 1, 0);
        }
        //Left
        else if (gameManager.direction == "left")
        {
            if (gameManager.currentCard.moneyStatLeft != 0)
                moneyStatusImpact.transform.localScale = new Vector3(1, 1, 0);
            if (gameManager.currentCard.energyStatLeft != 0)
                energyStatusImpact.transform.localScale = new Vector3(1, 1, 0);
            if (gameManager.currentCard.reputationStatLeft != 0)
                reputationStatusImpact.transform.localScale = new Vector3(1, 1, 0);
        }
        else
        {
            moneyStatusImpact.transform.localScale = new Vector3(0, 0, 0);
            energyStatusImpact.transform.localScale = new Vector3(0, 0, 0);
            reputationStatusImpact.transform.localScale = new Vector3(0, 0, 0);
        }
    }
}
