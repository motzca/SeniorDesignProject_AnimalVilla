using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static int MoneyStatus { get; private set; } = 50;
    public static int EnergyStatus { get; private set; } = 50;
    public static int ReputationStatus { get; private set; } = 50;
    public static readonly int MaxValue = 100;
    public static readonly int MinValue = 0;

    public GameObject cardGameObject;
    public CardController mainCardController;
    public SpriteRenderer cardSpriteRenderer;
    public ResourceManager resourceManager;
    public Vector2 defaultPositionCard = new Vector2(0, 1);
    public float movingSpeed;
    public float sideMargin;
    public float sideTrigger;
    public Color textColor;
    public float divideValue;

    public TMP_Text characterDialogue;
    public TMP_Text actionQuote;

    public string Direction { get; private set; }
    private string leftQuote;
    private string rightQuote;
    public Card CurrentCard { get; private set; }
    public Card testCard;

    void Start()
    {
        Card.OnLeftSwipe += HandleLeftSwipe;
        Card.OnRightSwipe += HandleRightSwipe;
        LoadCard(testCard);
        ResetCardPosition();
    }

    void OnDestroy()
    {
        Card.OnLeftSwipe -= HandleLeftSwipe;
        Card.OnRightSwipe -= HandleRightSwipe;
    }

    void Update()
    {
        HandleCardInput();
        UpdateCardPosition();
        UpdateDialogue();
    }

    private void HandleCardInput()
    {
        if (cardGameObject.transform.position.x > sideTrigger && Input.GetMouseButtonUp(0))
        {
            CurrentCard.ApplyRightEffect();
        }
        else if (cardGameObject.transform.position.x < -sideMargin && Input.GetMouseButtonUp(0))
        {
            CurrentCard.ApplyLeftEffect();
        }
        else
        {
            Direction = "none"; 
        }
    }

    private void UpdateCardPosition()
    {
        if (Input.GetMouseButton(0) && mainCardController.IsMouseOver)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cardGameObject.transform.position = pos;
        }
        else
        {
            cardGameObject.transform.position = Vector2.MoveTowards(cardGameObject.transform.position, defaultPositionCard, movingSpeed);
        }
    }

    private void UpdateDialogue()
    {
        textColor.a = Mathf.Min((Mathf.Abs(cardGameObject.transform.position.x) - sideMargin) / divideValue, 1);
        actionQuote.color = textColor;

        if (cardGameObject.transform.position.x < 0)
        {
            actionQuote.text = leftQuote;
        }
        else
        {
            actionQuote.text = rightQuote;
        }
    }

    private void HandleLeftSwipe(Card card)
    {
        MoneyStatus = Mathf.Clamp(MoneyStatus + card.moneyStatLeft, MinValue, MaxValue);
        EnergyStatus = Mathf.Clamp(EnergyStatus + card.energyStatLeft, MinValue, MaxValue);
        ReputationStatus = Mathf.Clamp(ReputationStatus + card.reputationStatLeft, MinValue, MaxValue);
        NewCard();
        Direction = "left";
    }

    private void HandleRightSwipe(Card card)
    {
        MoneyStatus = Mathf.Clamp(MoneyStatus + card.moneyStatRight, MinValue, MaxValue);
        EnergyStatus = Mathf.Clamp(EnergyStatus + card.energyStatRight, MinValue, MaxValue);
        ReputationStatus = Mathf.Clamp(ReputationStatus + card.reputationStatRight, MinValue, MaxValue);
        NewCard();
        Direction = "right";
    }

    public void LoadCard(Card card)
    {
        cardSpriteRenderer.sprite = resourceManager.sprites[(int)card.sprite];
        leftQuote = card.leftQuote;
        rightQuote = card.rightQuote;
        CurrentCard = card;
        characterDialogue.text = card.dialogue;
    }

    public void NewCard()
    {
        int rollDice = Random.Range(0, resourceManager.cards.Length);
        LoadCard(resourceManager.cards[rollDice]);
        ResetCardPosition();
    }

    private void ResetCardPosition()
    {
        cardGameObject.transform.position = defaultPositionCard;
    }
}
