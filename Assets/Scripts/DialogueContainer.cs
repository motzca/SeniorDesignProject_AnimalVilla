using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class DialogueContainer : ScriptableObject
{
    public List<DialogueNode> dialogueNodes;

    public Card GetNextCard(Card currentCard, string direction)
    {
        // Find the next node based on currentCard and direction
        DialogueNode nextNode = dialogueNodes.Find(node => node.CardId == currentCard.cardId && node.Direction == direction);

        // If a matching node is found, return the associated card
        if (nextNode != null)
        {
            return nextNode.Card;
        }
        else
        {
            // If no matching node is found, handle it here
            // For example, you might want to return a default card or a specific card for this case
            // Modify this part according to your requirements
            Debug.LogWarning($"No matching node found for card {currentCard.cardId} and direction {direction}. Returning a default card.");

            // Example: Return the first card in the dialogueNodes list as a default
            if (dialogueNodes.Count > 0)
            {
                return dialogueNodes[0].Card;
            }
            else
            {
                // If dialogueNodes is empty, return a default card (you need to define what your default card is)
                Debug.LogError("No cards available in dialogueNodes. Please add cards to the dialogue container.");
                return null;
            }
        }
    }
}

