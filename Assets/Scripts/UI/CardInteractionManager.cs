using UnityEngine;
using UnityEngine.UI;

public class CardInteractionManager : MonoBehaviour
{
    [SerializeField] private Text leftSwipeChoice;
    [SerializeField] private Text rightSwipeChoice;
    [SerializeField] private Image npcImage;
    [SerializeField] private Text npcName;
    [SerializeField] private Text description;
    private DeckManager deckManager;
    private CardSetUpManager currentCardSetUp;

    private void Start()
    {
        deckManager = FindObjectOfType<DeckManager>();
        LoadNextCard();
    }

    private void LoadNextCard()
    {
        currentCardSetUp = deckManager.GetNextCard();
        SetUpCardUI();
    }

    private void SetUpCardUI()
    {
        leftSwipeChoice.text = currentCardSetUp.LeftChoice;
        rightSwipeChoice.text = currentCardSetUp.RightChoice;
        npcImage.sprite = currentCardSetUp.CharacterSprite;
        npcName.text = currentCardSetUp.CharacterName;
        description.text = currentCardSetUp.Description;
    }

    public void SwipeLeft()
    {
        deckManager.ApplyChoiceEffects(currentCardSetUp.LeftChoiceEffects);
        LoadNextCard();
    }

    public void SwipeRight()
    {
        deckManager.ApplyChoiceEffects(currentCardSetUp.RightChoiceEffects);
        LoadNextCard();
    }
}
