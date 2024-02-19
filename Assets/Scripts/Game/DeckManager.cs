using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public CardSetUpManager[] cardSetUps;
    private int currentCardIndex = 0;

    public void ApplyChoiceEffects(ChangedIndicatorsInfo choiceEffects)
    {
        // Apply the effects for the players choice
        foreach (var effect in choiceEffects.IndicatorChanges)
        {
            // Add logic to modify the games health, wealth, happiness, etc.
        }
    }

    public CardSetUpManager GetNextCard()
    {
        if (currentCardIndex < cardSetUps.Length - 1)
        {
            currentCardIndex++;
        }
        else
        {
            currentCardIndex = 0; // currently just resets to the first card
        }

        return cardSetUps[currentCardIndex];
    }
}
