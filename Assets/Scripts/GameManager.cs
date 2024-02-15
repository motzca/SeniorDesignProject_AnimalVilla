using UnityEngine;
using System.Collections.Generic;
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
    public List<CardData> cardDataList;

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
    CardData cardData = FindCardDataByID(card.cardId);
    if (cardData != null)
    {
        ApplyCardEffect(cardData);
        Direction = "left";
    }
    else
    {
        Debug.LogError("Card data not found for card ID: " + card.cardId);
    }
}

private void HandleRightSwipe(Card card)
{
    CardData cardData = FindCardDataByID(card.cardId);
    if (cardData != null)
    {
        ApplyCardEffect(cardData);
        Direction = "right";
    }
    else
    {
        Debug.LogError("Card data not found for card ID: " + card.cardId);
    }
}


private void ApplyCardEffect(CardData cardData)
{
    MoneyStatus = Mathf.Clamp(MoneyStatus + cardData.moneyStatLeft, MinValue, MaxValue);
    EnergyStatus = Mathf.Clamp(EnergyStatus + cardData.energyStatLeft, MinValue, MaxValue);
    ReputationStatus = Mathf.Clamp(ReputationStatus + cardData.reputationStatLeft, MinValue, MaxValue);
    NewCard();
}

 public void LoadCard(Card card)
    {
        CardData cardData = FindCardDataByID(card.cardId);

        if (cardData != null)
        {
            // Update the card sprite renderer
            cardSpriteRenderer.sprite = resourceManager.sprites[cardData.spriteIndex];

            // Update other card properties
            leftQuote = cardData.leftQuote;
            rightQuote = cardData.rightQuote;
            CurrentCard = card;
            characterDialogue.text = cardData.dialogue;
        }
        else
        {
            Debug.LogError("Card data not found for card ID: " + card.cardId);
        }
    }

    public CardData FindCardDataByID(int cardID)
    {
        foreach (CardData data in resourceManager.cardData)
        {
            if (data.cardId == cardID)
            {
                return data;
            }
        }
        return null;
    }

    public void NewCard()
    {
        if (CurrentCard != null)
        {
            CardData nextCardData = null;

            // Determine the next card data based on the swipe direction
            if (Direction == "right")
            {
                nextCardData = FindCardDataByID(CurrentCard.cardId); // Change this line to fit your specific logic
            }
            else if (Direction == "left")
            {
                nextCardData = FindCardDataByID(CurrentCard.cardId); // Change this line to fit your specific logic
            }

            // Load the next card if found
            if (nextCardData != null)
            {
                // Create a new card object and load it
                Card nextCard = ScriptableObject.CreateInstance<Card>();
                nextCard.cardId = nextCardData.cardId; // Set the ID of the next card
                LoadCard(nextCard);
                ResetCardToDefault();
            }
            else
            {
                Debug.LogError("Next card data not found for current card ID: " + CurrentCard.cardId);
            }
        }
        else
        {
            Debug.LogError("Current card is null.");
        }
    }

    public void LoadCardData()
    {
        string filePath = Application.streamingAssetsPath + "/Cards/CardHardData.json";

        if (System.IO.File.Exists(filePath))
        {
            string dataAsJson = System.IO.File.ReadAllText(filePath);
            cardDataList = JsonUtility.FromJson<List<CardData>>(dataAsJson);
        }
        else
        {
            Debug.LogError("Cannot find card data JSON file at path: " + filePath);
        }
    }
    private void ResetCardToDefault()
    {
        cardGameObject.transform.position = defaultPositionCard;
        cardGameObject.transform.rotation = Quaternion.identity;
    }

    public Card CreateCard(int cardId)
    {
        CardData cardData = cardDataList.Find(x => x.cardId == cardId);
        if (cardData != null)
        {
            Card newCard = ScriptableObject.CreateInstance<Card>();
            newCard.cardId = cardData.cardId;
            // Set any other properties of the Card instance here if needed
            return newCard;
        }
        else
        {
            Debug.LogError("Card data not found for ID: " + cardId);
            return null;
        }
    }
}
