using UnityEngine;

[CreateAssetMenu]
public class Card : ScriptableObject
{
    public CardSprite sprite;
    public string cardName;
    public string dialogue;
    public int cardId;

    public int moneyStatLeft;
    public int energyStatLeft;
    public int reputationStatLeft;

    public int moneyStatRight;
    public int energyStatRight;
    public int reputationStatRight;

    private DialogueNode dialogueNode;

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

    public void SetDialogueNode(DialogueNode node)
    {
        dialogueNode = node;
    }
    public DialogueOption[] GetOptions()
    {
        return dialogueNode != null ? dialogueNode.Options : null;
    }
}
