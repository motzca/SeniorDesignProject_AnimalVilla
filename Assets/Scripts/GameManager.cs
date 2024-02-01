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
    public Vector2 defaultPositionCard;
    public DialogueContainer dialogueContainer;

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
        ResetCardToDefault();
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
            Direction = "right";
            ResetCardToDefault();
        }
        else if (cardGameObject.transform.position.x < -sideMargin && Input.GetMouseButtonUp(0))
        {
            CurrentCard.ApplyLeftEffect();
            Direction = "left";
            ResetCardToDefault();
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
            if (!isSwiping)
            {
                verticalSwipeDirection = Random.Range(0, 2) * 2 - 1;
                isSwiping = true;
            }

            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float horizontalDistance = mousePosition.x - defaultPositionCard.x;

            float verticalMovement = horizontalDistance * verticalSwipeDirection * 0.8f;

            Vector2 newPosition = new Vector2(mousePosition.x, defaultPositionCard.y + verticalMovement);
            cardGameObject.transform.position = newPosition;

            float rotationAngle = horizontalDistance * 10.0f; 
            cardGameObject.transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
        }
        else if (isSwiping)
        {
            cardGameObject.transform.position = Vector2.MoveTowards(cardGameObject.transform.position, defaultPositionCard, movingSpeed);
            cardGameObject.transform.rotation = Quaternion.Slerp(cardGameObject.transform.rotation, Quaternion.identity, movingSpeed * Time.deltaTime);
            isSwiping = false;
        }
    }

    private int verticalSwipeDirection = 1;
    private bool isSwiping = false; 


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
        ApplyCardEffect(card, card.moneyStatLeft, card.energyStatLeft, card.reputationStatLeft);
        Direction = "left";
    }

    private void HandleRightSwipe(Card card)
    {
        ApplyCardEffect(card, card.moneyStatRight, card.energyStatRight, card.reputationStatRight);
        Direction = "right";
    }


    public void ResetCardToDefault()
    {
        cardGameObject.transform.position = defaultPositionCard;
        cardGameObject.transform.rotation = Quaternion.identity;
    }


    private void ApplyCardEffect(Card card, int moneyStat, int energyStat, int reputationStat)
    {
        MoneyStatus = Mathf.Clamp(MoneyStatus + moneyStat, MinValue, MaxValue);
        EnergyStatus = Mathf.Clamp(EnergyStatus + energyStat, MinValue, MaxValue);
        ReputationStatus = Mathf.Clamp(ReputationStatus + reputationStat, MinValue, MaxValue);
        NewCard();
    }

    public void NewCard(int optionIndex)
    {
        if (resourceManager.cards != null && resourceManager.cards.Length > 0)
        {
            int rollDice = Random.Range(0, resourceManager.cards.Length);
            LoadCard(resourceManager.cards[rollDice], optionIndex);
            ResetCardToDefault();
        }
        else
        {
            Debug.LogError("Resource Manager cards array is null or empty. Please check your setup.");
        }
    }

    public void LoadCard(Card card, int optionIndex)
    {
        if (card != null)
        {
            cardSpriteRenderer.sprite = resourceManager.sprites[(int)card.sprite];
            leftQuote = card.GetOptions()[optionIndex].Quote;
            rightQuote = card.GetOptions()[optionIndex].Quote;
            CurrentCard = card;
            characterDialogue.text = card.GetOptions()[optionIndex].SpeechText;
        }
        else
        {
            Debug.LogError("Attempting to load a null card. Please check your card data.");
        }
    }

}
