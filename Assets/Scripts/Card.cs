using UnityEngine;

[CreateAssetMenu]
public class Card : ScriptableObject
{
    public CardSprite sprite;
    public string leftQuote;
    public string rightQuote;
    public string cardName;
    public string leftSwipeBlockName;
    public string rightSwipeBlockName;
    public string dialogue;
    public int cardId;

    public int moneyStatLeft;
    public int energyStatLeft;
    public int reputationStatLeft;

    public int moneyStatRight;
    public int energyStatRight;
    public int reputationStatRight;

    public delegate void CardSwipeAction(Card card);
    public static event CardSwipeAction OnLeftSwipe;
    public static event CardSwipeAction OnRightSwipe;

    public void ApplyLeftEffect()
    {
        Debug.Log(cardName + " swiped left");
        OnLeftSwipe?.Invoke(this);
    }

    public void ApplyRightEffect()
    {
        Debug.Log(cardName + " swiped right");
        OnRightSwipe?.Invoke(this);
    }
}
