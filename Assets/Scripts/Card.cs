using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    //Dialog values
    public string sentenceText;
    public DialogNode leftChoiceNode;
    public DialogNode rightChoiceNode;

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
        GameManager.characterDialogue.text = leftChoiceNode.card.sentenceText;
        GameManager.dialogManager.LoadDialogNode(leftChoiceNode);


        // Appending the values for the left
        GameManager.moneyStatus += moneyStatLeft;
        GameManager.energyStatus += energyStatLeft;
        GameManager.reputationStatus += reputationStatLeft;
    }

    public void Right()
    {
        Debug.Log(cardName + "swipped right");
        GameManager.characterDialogue.text = rightChoiceNode.card.sentenceText;
        GameManager.dialogManager.LoadDialogNode(rightChoiceNode);

        // Appending the values for the right
        GameManager.moneyStatus += moneyStatRight;
        GameManager.energyStatus += energyStatRight;
        GameManager.reputationStatus += reputationStatRight;
    }
}
