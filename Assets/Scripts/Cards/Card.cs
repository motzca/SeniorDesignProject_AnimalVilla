using UnityEngine;

[CreateAssetMenu]
public class Card : ScriptableObject
{
    public CardSprite sprite;
    public string leftQuote;
    public string rightQuote;
    public string cardName;
    public string dialogue;
    public int cardId;

    public int moneyStatLeft;
    public int energyStatLeft;
    public int reputationStatLeft;

    public int moneyStatRight;
    public int energyStatRight;
    public int reputationStatRight;

    public int endingMoney { get; set; }
    public int endingEnergy { get; set; }
    public int endingReputation { get; set; }
}
