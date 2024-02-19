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

    public GameObject cardGameObject;
    public CardController mainCardController;
    public SpriteRenderer cardSpriteRenderer;
    public ResourceManager resourceManager;
    public Vector2 defaultPositionCard;

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
        LoadCard(testCard);
        ResetCardToDefault();
    }

    void Update()
    {
        HandleCardInput();
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
        if (swipedRight)
        {
            ApplyCardEffect(CurrentCard, true);
            Direction = "right";
        }
        else
        {
            ApplyCardEffect(CurrentCard, false);
            Direction = "left";
        }
        ResetCardToDefault();
        NewCard();
    }

    private void ApplyCardEffect(Card card, bool swipedRight)
    {
        int moneyStat = swipedRight ? card.moneyStatRight : card.moneyStatLeft;
        int energyStat = swipedRight ? card.energyStatRight : card.energyStatLeft;
        int reputationStat = swipedRight ? card.reputationStatRight : card.reputationStatLeft;

        MoneyStatus = Mathf.Clamp(MoneyStatus + moneyStat, MinValue, MaxValue);
        EnergyStatus = Mathf.Clamp(EnergyStatus + energyStat, MinValue, MaxValue);
        ReputationStatus = Mathf.Clamp(ReputationStatus + reputationStat, MinValue, MaxValue);
    }

    public void LoadCard(Card card)
    {
        cardSpriteRenderer.sprite = resourceManager.sprites[(int)card.sprite];
        leftQuote = card.leftQuote;
        rightQuote = card.rightQuote;
        CurrentCard = card;
        characterDialogue.text = card.dialogue;
    }

    private void NewCard()
    {
        int rollDice = Random.Range(0, resourceManager.cards.Length);
        LoadCard(resourceManager.cards[rollDice]);
    }

    private void ResetCardToDefault()
    {
        cardGameObject.transform.position = defaultPositionCard;
        cardGameObject.transform.rotation = Quaternion.identity;
    }
}