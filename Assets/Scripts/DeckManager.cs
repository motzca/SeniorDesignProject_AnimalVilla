using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public CardSetUpManager[] cardSetUps;
    private int currentCardIndex = 0;

    public void ApplyChoiceEffects(ChangedIndicatorsInfo choiceEffects)
    {
        // Apply the effects from the choice
        foreach (var effect in choiceEffects.IndicatorChanges)
        {
            // Implement logic to modify the game state based on the effect
            // This should be increasing/decreasing health, wealth, happiness.
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
