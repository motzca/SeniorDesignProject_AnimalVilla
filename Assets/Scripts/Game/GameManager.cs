using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static int MoneyStatus { get; private set; } = 50;
    public static int EnergyStatus { get; private set; } = 50;
    public static int ReputationStatus { get; private set; } = 50;
    public static readonly int MaxValue = 100;
    public static readonly int MinValue = 0;
    public int endingMoney = 0;
    public int endingEnergy= 0;
    public int endingReputation = 0;
    public bool isEndingDisplayed;

    public GameObject cardGameObject;
    public CardController mainCardController;
    public SpriteRenderer cardSpriteRenderer;
    public ResourceManager resourceManager;
    public Vector2 defaultPositionCard;
    public EndingManager endingManager;

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

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        defaultPositionCard = new Vector2(0, cardGameObject.transform.position.y);
        cardGameObject.transform.position = defaultPositionCard;
        LoadCard(testCard);
        ResetCardToDefault();
        endingManager = FindObjectOfType<EndingManager>();
        Debug.Log($"Start: Card X position set to {cardGameObject.transform.position.x}");
    }

    void Update()
    {
        HandleCardInput();
        UpdateDialogue();
        endingManager.CheckForStatEndings(MoneyStatus, EnergyStatus, ReputationStatus, endingMoney, endingEnergy, endingReputation);
    }

    private void UpdateDialogue()
    {
        float positionVariance = 1.0f;

        if (Mathf.Abs(cardGameObject.transform.position.x - defaultPositionCard.x) <= positionVariance)
        {
            actionQuote.text = "Swipe left or right";
        }
        else
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
    }

    public void UpdateMoney(int change)
    {
        MoneyStatus = Mathf.Clamp(MoneyStatus + change, MinValue, MaxValue);
    }

    public void UpdateEnergy(int change)
    {
        EnergyStatus = Mathf.Clamp(EnergyStatus + change, MinValue, MaxValue);
    }

    public void UpdateReputation(int change)
    {
        ReputationStatus = Mathf.Clamp(ReputationStatus + change, MinValue, MaxValue);
    }

    private void HandleCardInput()
    {
        if (cardGameObject.transform.position.x > sideTrigger && Input.GetMouseButtonUp(0))
        {
            ProcessCardSwipe(true);
        }
        else if (cardGameObject.transform.position.x < -sideMargin && Input.GetMouseButtonUp(0))
        {
            ProcessCardSwipe(false);
        }
    }

    private void ProcessCardSwipe(bool swipedRight)
    {
        Direction = swipedRight ? "right" : "left";
        ApplyCardEffect(CurrentCard, swipedRight);
    }

    public void ProcessSwipeResult(bool swipedRight, bool isSwipeClear)
    {
        if (isSwipeClear)
        {
            ApplyCardEffect(CurrentCard, swipedRight); 
            NewCard();
        }
        else
        {
            Debug.Log("Retaining current card due to unclear swipe.");
            ResetCardToDefault();
        }
    }

    private void ApplyCardEffect(Card card, bool swipedRight)
    {
        int moneyStat = swipedRight ? card.moneyStatRight : card.moneyStatLeft;
        int energyStat = swipedRight ? card.energyStatRight : card.energyStatLeft;
        int reputationStat = swipedRight ? card.reputationStatRight : card.reputationStatLeft;

        MoneyStatus = Mathf.Clamp(MoneyStatus + moneyStat, MinValue, MaxValue);
        EnergyStatus = Mathf.Clamp(EnergyStatus + energyStat, MinValue, MaxValue);
        ReputationStatus = Mathf.Clamp(ReputationStatus + reputationStat, MinValue, MaxValue);

        if (MoneyStatus <= 0 || EnergyStatus <= 0 || ReputationStatus <= 0)
        {
            LoadStatEndingCard();
            return;
        }
    }

    public void LoadCard(Card card)
    {
        cardSpriteRenderer.sprite = resourceManager.sprites[(int)card.sprite];
        leftQuote = card.leftQuote;
        rightQuote = card.rightQuote;
        CurrentCard = card;
        characterDialogue.text = card.dialogue;
        actionQuote.text = "Swipe left or right";
    }

    public void LoadStatEndingCard()
    {
        Debug.Log("Load Stat Ending Card");
        if (MoneyStatus <= 0)
        {
            LoadCard(resourceManager.cards[endingMoney]);
        }
        else if (EnergyStatus <= 0)
        {
            LoadCard(resourceManager.cards[endingEnergy]);
        }
        else if (ReputationStatus <= 0)
        {
            LoadCard(resourceManager.cards[endingReputation]);
        }
    }

    private void NewCard()
    {
        if (!isEndingDisplayed)
        {
            int rollDice = Random.Range(0, resourceManager.cards.Length);
            Debug.Log("New Card");
            LoadCard(resourceManager.cards[rollDice]);
        }
    }

    private void ResetCardToDefault()
    {
        cardGameObject.transform.position = new Vector2(0, cardGameObject.transform.position.y);
        cardGameObject.transform.rotation = Quaternion.identity;
        actionQuote.text = "Swipe left or right";
    }
}
