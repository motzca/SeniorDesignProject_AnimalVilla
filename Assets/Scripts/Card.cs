using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;
[CreateAssetMenu]

public class Card : ScriptableObject
{
    // Basic card values
    public CardSprite sprite;
    public string leftQuote;
    public string rightQuote;
    public string cardName;
    public string dialogue;
    public int cardId;

    // Stat values - left swipe
    public int moneyStatLeft;
    public int energyStatLeft;
    public int reputationStatLeft;

    // Stat values - right swipe
    public int moneyStatRight;
    public int energyStatRight;
    public int reputationStatRight;

    public void Left()
    {
        Debug.Log(cardName + "swipped left");

        // Appending the values for the left
        GameManager.moneyStatus += moneyStatLeft;
        GameManager.energyStatus += energyStatLeft;
        GameManager.reputationStatus += reputationStatLeft;
    }

    public void Right()
    {
        Debug.Log(cardName + "swipped right");

        // Appending the values for the right
        GameManager.moneyStatus += moneyStatRight;
        GameManager.energyStatus += energyStatRight;
        GameManager.reputationStatus += reputationStatRight;
    }
}
