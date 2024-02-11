using UnityEngine;
using TMPro;
using Fungus;

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
    public Flowchart flowchart;

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

    private int currentCardIndex = 0;
    private Card[] storyCards;

    void Start()
    {
        LoadNextCard();
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
    public void LoadCard(Card card)
    {
        cardSpriteRenderer.sprite = cardData.sprite;
        if (flowchart != null)
        {
            var block = flowchart.FindBlock(cardData.fungusBlockName);
            if (block != null)
            {
                var dialogueText = block.GetComponentsInChildren<TextMeshProUGUI>()[0];
                characterDialogue.text = dialogueText.text;
            }
            else
            {
                Debug.LogWarning("Fungus block not found for card: " + cardData.cardName);
            }
        }
        else
        {
            Debug.LogError("Flowchart reference not set in GameManager.");
        }
    }

    public void NewCard()
    {
        int rollDice = Random.Range(0, resourceManager.cards.Length);
        LoadCard(resourceManager.cards[rollDice]);
        ResetCardToDefault();
    }

    private void LoadNextCard()
    {
        if (currentCardIndex < storyCards.Length)
        {
            LoadCard(storyCards[currentCardIndex]);
            currentCardIndex++;
        }
        else
        {
            Debug.Log("End of story reached.");
            // Handle end of story, e.g., display ending screen
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

    private void LoadNextCard()
    {
        if (currentCardIndex < storyCards.Length)
        {
            LoadCard(storyCards[currentCardIndex]);
            currentCardIndex++;
        }
        else
        {
            Debug.Log("End of story reached.");
            // Here we'll handle end of story, e.g., display ending screen
        }
    }

    private void HandleLeftSwipe(Card card)
    {
        LoadNextCard();
    }

    private void HandleRightSwipe(Card card)
    {
        LoadNextCard();
    }

    private void ApplyCardEffect(Card card, int moneyStat, int energyStat, int reputationStat)
    {
        MoneyStatus = Mathf.Clamp(MoneyStatus + moneyStat, MinValue, MaxValue);
        EnergyStatus = Mathf.Clamp(EnergyStatus + energyStat, MinValue, MaxValue);
        ReputationStatus = Mathf.Clamp(ReputationStatus + reputationStat, MinValue, MaxValue);
        NewCard();
    }


    private void ExecuteFungusBlock(string blockName)
    {
        if (flowchart != null)
        {
            if (!string.IsNullOrEmpty(blockName))
            {
                flowchart.ExecuteBlock(blockName);
            }
            else
            {
                Debug.LogError("Block name is empty or null.");
            }
        }
        else
        {
            Debug.LogError("Flowchart reference not set in GameManager.");
        }
    }

    private void ResetCardToDefault()
    {
        cardGameObject.transform.position = defaultPositionCard;
        cardGameObject.transform.rotation = Quaternion.identity;
    }
}

