using UnityEngine;
using UnityEngine.UI;

public class CardSetUpManager : MonoBehaviour
{
    public string LeftChoice { get; private set; }
    public string RightChoice { get; private set; }
    public string Description { get; private set; }
    public Sprite CharacterSprite { get; private set; }
    public string CharacterName { get; private set; }
    public ChangedIndicatorsInfo LeftChoiceEffects { get; private set; }
    public ChangedIndicatorsInfo RightChoiceEffects { get; private set; }

    public void SetUpCard(string leftChoice, string rightChoice, string description, Sprite characterSprite, string characterName, ChangedIndicatorsInfo leftEffects, ChangedIndicatorsInfo rightEffects)
    {
        LeftChoice = leftChoice;
        RightChoice = rightChoice;
        Description = description;
        CharacterSprite = characterSprite;
        CharacterName = characterName;
        LeftChoiceEffects = leftEffects;
        RightChoiceEffects = rightEffects;
    }
}
