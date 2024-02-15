using UnityEngine;

[CreateAssetMenu]
public class Card : ScriptableObject
{
    public int cardId;

    public delegate void CardSwipeAction(Card card);
    public static event CardSwipeAction OnLeftSwipe;
    public static event CardSwipeAction OnRightSwipe;

    public void ApplyLeftEffect()
    {
        Debug.Log("Card with ID " + cardId + " swiped left");
        OnLeftSwipe?.Invoke(this);
    }

    public void ApplyRightEffect()
    {
        Debug.Log("Card with ID " + cardId + " swiped right");
        OnRightSwipe?.Invoke(this);
    }
}
