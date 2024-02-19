using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text leftSwipeChoice;
    [SerializeField] private Text rightSwipeChoice;
    [SerializeField] private Image npcImage;
    [SerializeField] private Text npcName;
    [SerializeField] private Text description;

    public void UpdateCardUI(CardSetUpManager currentCardSetUp)
    {
        leftSwipeChoice.text = currentCardSetUp.LeftChoice;
        rightSwipeChoice.text = currentCardSetUp.RightChoice;
        npcImage.sprite = currentCardSetUp.CharacterSprite;
        npcName.text = currentCardSetUp.CharacterName;
        description.text = currentCardSetUp.Description;
    }
}
