using UnityEngine;
using TMPro;
using Fungus;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static int MoneyStatus { get; private set; } = 50;
    public static int EnergyStatus { get; private set; } = 50;
    public static int ReputationStatus { get; private set; } = 50;
    public readonly int MaxValue = 100;
    public readonly int MinValue = 0;

    public int pendingMoneyChange;
    public int pendingEnergyChange;
    public int pendingReputationChange;

    public delegate void StatReachedZero();
    public static event StatReachedZero OnMoneyZero;
    public static event StatReachedZero OnEnergyZero;
    public static event StatReachedZero OnReputationZero;

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
    public TMP_Text MoneyNumber;
    public TMP_Text ReputationNumber;
    public TMP_Text EnergyNumber;
    

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
        endingManager = FindObjectOfType<EndingManager>();

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

        if (endingManager != null)
        {
            // Activate the endingManager if it's not active
            if (!endingManager.gameObject.activeSelf)
            {
                endingManager.gameObject.SetActive(true);
                Debug.Log("EndingManager activated.");
            }
            else
            {
                Debug.Log("EndingManager is already active.");
            }
        }
        else
        {
            Debug.LogError("EndingManager not found in the scene.");
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
        CheckForEndings();
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

    public void CheckForEndings() {
        if (MoneyStatus <= 0 && OnMoneyZero != null) {
            OnMoneyZero.Invoke();
            Debug.Log("Check for ending (money)");
        }
        else if (EnergyStatus <= 0 && OnEnergyZero != null) {
            OnEnergyZero.Invoke();
        }
        else if (ReputationStatus <= 0 && OnReputationZero != null) {
            OnReputationZero.Invoke();
        }
    }

    private void ProcessCardSwipe(bool swipedRight)
    {
        Direction = swipedRight ? "right" : "left";

        ApplyCardEffect(CurrentCard, swipedRight);
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
        
        UpdateStatsVariables();
        
    }

    public void LoadCard(Card card)
    {
        flowchart.ExecuteBlock(nextCall);
        cardSpriteRenderer.sprite = resourceManager.sprites[(int)card.sprite];
        leftQuote = flowchart.GetStringVariable("LeftActionQuote");
        rightQuote = flowchart.GetStringVariable("RightActionQuote");;
        MoneyNumber.text = MoneyStatus.ToString();
        ReputationNumber.text = ReputationStatus.ToString();
        EnergyNumber.text = EnergyStatus.ToString();
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

    public void UpdateStatsVariables()
    {
        //Recieve variable numbers from Fungus Flowchart
        pendingMoneyChange = int.Parse(flowchart.GetStringVariable("ChangeMoney"));
        pendingEnergyChange = int.Parse(flowchart.GetStringVariable("ChangeEnergy"));
        pendingReputationChange = int.Parse(flowchart.GetStringVariable("ChangeStatus"));

        //Calculate the effects
        CalculatePendingEffects(pendingMoneyChange, pendingEnergyChange, pendingReputationChange);
        
        //Display new numbers for status to the player
        MoneyNumber.text = MoneyStatus.ToString();
        ReputationNumber.text = ReputationStatus.ToString();
        EnergyNumber.text = EnergyStatus.ToString();
    }
   
    public void CalculatePendingEffects(int pendingMoneyChange, int pendingEnergyChange, int pendingReputationChange)
    {
        //Numbers that need to be added to Status Item, will be positive in the flow chart
        //Numbers that need to be subtracted to Status Item, will be negative in the flow chart
        MoneyStatus = MoneyStatus + pendingMoneyChange;
        EnergyStatus = EnergyStatus + pendingEnergyChange;
        ReputationStatus = ReputationStatus + pendingReputationChange;
    }
}