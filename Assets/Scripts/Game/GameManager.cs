using UnityEngine;
using TMPro;
using Fungus;

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
    public Flowchart flowchart;
    public Card testCard;
    public string nextCall;

    [SerializeField] private AudioSource backgroundMusicSource; 

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

        int moneyStat = swipedRight ? card.moneyStatRight : card.moneyStatLeft;
        int energyStat = swipedRight ? card.energyStatRight : card.energyStatLeft;
        int reputationStat = swipedRight ? card.reputationStatRight : card.reputationStatLeft;

        MoneyStatus = Mathf.Clamp(MoneyStatus + moneyStat, MinValue, MaxValue);
        EnergyStatus = Mathf.Clamp(EnergyStatus + energyStat, MinValue, MaxValue);
        ReputationStatus = Mathf.Clamp(ReputationStatus + reputationStat, MinValue, MaxValue);
    }

    public void LoadCard(Card card)
    {
        flowchart.ExecuteBlock(nextCall);
        cardSpriteRenderer.sprite = resourceManager.sprites[(int)card.sprite];
        leftQuote = flowchart.GetStringVariable("LeftActionQuote");
        rightQuote = flowchart.GetStringVariable("RightActionQuote");;
        CurrentCard = card;
        //characterDialogue.text = flowchart.GetStringVariable("CharacterDialogue");
        //actionQuote.text = "Swipe left or right";
        //flowchart.SetStringVariable("CharacterDialogue", card.dialogue);
        //flowchart.SetStringVariable("LeftActionQuote", card.leftQuote);
        //flowchart.SetStringVariable("RightActionQuote", card.rightQuote);
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
}
