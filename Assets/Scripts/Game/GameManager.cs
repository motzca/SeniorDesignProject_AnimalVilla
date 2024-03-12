using UnityEngine;
using TMPro;
using Fungus;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public string Direction { get; private set; }

    public static int MoneyStatus { get; private set; } = 50;
    public static int EnergyStatus { get; private set; } = 50;
    public static int ReputationStatus { get; private set; } = 50;
    public static readonly int MaxValue = 100;
    public static readonly int MinValue = 0;
    private int endingMoneyCardId = 0;
    private int endingEnergyCardId = 0;
    private int endingReputationCardId = 0;


    private int pendingMoneyChange;
    private int pendingEnergyChange;
    private int pendingReputationChange;

    public GameObject cardGameObject;
    public SpriteRenderer cardSpriteRenderer;
    public ResourceManager resourceManager;
    public Vector2 defaultPositionCard;

    public float sideMargin;
    public float sideTrigger;
    public Color textColor;
    public float divideValue;

    public TMP_Text characterDialogue;
    public TMP_Text actionQuote;

    private string leftQuote;
    private string rightQuote;
    public Card CurrentCard { get; private set; }
    public Flowchart flowchart;
    public Card testCard;
    public string nextCall;

    [SerializeField] private AudioSource backgroundMusicSource; 

    public delegate void StatReachedZero(int cardId);
    public static event StatReachedZero OnMoneyZero;
    public static event StatReachedZero OnEnergyZero;
    public static event StatReachedZero OnReputationZero;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAndApplyUserSettings();
        }
    }

    void Start()
    {
        defaultPositionCard = new Vector2(0, cardGameObject.transform.position.y);
        cardGameObject.transform.position = defaultPositionCard;
        LoadCard(testCard);
        ResetCardToDefault();
    }

    void Update()
    {
        HandleCardInput();
        UpdateDialogue();

        if (MoneyStatus <= 0 && OnMoneyZero != null)
        {
            OnMoneyZero(endingMoneyCardId);
        }
        else if (EnergyStatus <= 0 && OnEnergyZero != null)
        {
            OnEnergyZero(endingEnergyCardId);
        }
        else if (ReputationStatus <= 0 && OnReputationZero != null)
        {
            OnReputationZero(endingReputationCardId);
        }
    }

    private void LoadAndApplyUserSettings()
    {
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        AdjustMusicVolume(savedVolume);
    }

    private void AdjustMusicVolume(float volume)
    {
        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.volume = volume;
        }
    }

    private void UpdateDialogue()
    {
        float positionVariance = 1.0f;

        if (Mathf.Abs(cardGameObject.transform.position.x - defaultPositionCard.x) <= positionVariance)
        {
            actionQuote.text = flowchart.GetStringVariable("CharacterDialogue");
        }
        else
        {
            textColor.a = Mathf.Min((Mathf.Abs(cardGameObject.transform.position.x) - sideMargin) / divideValue, 1);
            actionQuote.color = textColor;
            actionQuote.text = flowchart.GetStringVariable("CharacterDialogue");

            if (cardGameObject.transform.position.x < 0)
            {
                actionQuote.text = leftQuote;
                actionQuote.text = flowchart.GetStringVariable("LeftActionQuote");
            }
            else
            {
                actionQuote.text = flowchart.GetStringVariable("RightActionQuote");
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
            UpdateMoney(pendingMoneyChange);
            UpdateEnergy(pendingEnergyChange);
            UpdateReputation(pendingReputationChange);

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
        if(swipedRight != true)
        {
            nextCall = flowchart.GetStringVariable("LeftChoice");
        }
        else
        {
            nextCall = flowchart.GetStringVariable("RightChoice");
        }

        pendingMoneyChange = swipedRight ? card.moneyStatRight : card.moneyStatLeft;
        pendingEnergyChange = swipedRight ? card.energyStatRight : card.energyStatLeft;
        pendingReputationChange = swipedRight ? card.reputationStatRight : card.reputationStatLeft;
    }

    public void LoadCard(Card card)
    {
        flowchart.ExecuteBlock(nextCall);
        cardSpriteRenderer.sprite = resourceManager.sprites[(int)card.sprite];
        leftQuote = flowchart.GetStringVariable("LeftActionQuote");
        rightQuote = flowchart.GetStringVariable("RightActionQuote");;
        CurrentCard = card;
    }

    private void NewCard()
    {
        int rollDice = Random.Range(0, resourceManager.cards.Length);
        LoadCard(resourceManager.cards[rollDice]);
    }

    private void ResetCardToDefault()
    {
        cardGameObject.transform.position = new Vector2(0, cardGameObject.transform.position.y);
        cardGameObject.transform.rotation = Quaternion.identity;
        actionQuote.text = "Swipe left or right";
    }

    public void SetPendingEffects(int moneyChange, int energyChange, int reputationChange)
    {
        pendingMoneyChange = moneyChange;
        pendingEnergyChange = energyChange;
        pendingReputationChange = reputationChange;
    }
}
