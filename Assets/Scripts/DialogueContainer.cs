using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class DialogueContainer : ScriptableObject
{
    public List<DialogueNode> dialogueNodes;

    public Card GetNextCard(Card currentCard, string direction, int optionIndex)
    {
        DialogueNode nextNode = dialogueNodes.Find(node => node.CardId == currentCard.cardId && node.Direction == direction);

        if (nextNode != null)
        {
            if (optionIndex >= 0 && optionIndex < nextNode.Options.Length)
            {
                return nextNode.Options[optionIndex].ResultCard;
            }
            else
            {
                Debug.LogError($"Invalid optionIndex {optionIndex} for card {currentCard.cardId}. No matching option found.");
            }
        }
        else
        {
            Debug.LogWarning($"No matching node found for card {currentCard.cardId} and direction {direction}. Returning a default card.");
        }

        // Return a default card if no matching node or option is found
        return null;
    }
}


