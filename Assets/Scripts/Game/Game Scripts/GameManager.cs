using UnityEngine;
using System.Collections;
using TMPro;
using Fungus;

public class GameManager : MonoBehaviour
{
    // Singleton Pattern
    public static GameManager Instance { get; private set; }

    // Static Properties
    public static int MoneyStatus { get; private set; } = 50;
    public static int EnergyStatus { get; private set; } = 50;
    public static int ReputationStatus { get; private set; } = 50;
    public static readonly int MaxValue = 100;
    public static readonly int MinValue = 0;

    // Events
    public delegate void StatReachedZero();
    public static event StatReachedZero OnMoneyZero;
    public static event StatReachedZero OnEnergyZero;
    public static event StatReachedZero OnReputationZero;

    // UI Elements
    public TMP_Text characterDialogue;
    public TMP_Text actionQuote;
    public TMP_Text MoneyNumber;
    public TMP_Text ReputationNumber;
    public TMP_Text EnergyNumber;

    // Card Elements
    public GameObject cardGameObject;
    public CardController mainCardController;
    public SpriteRenderer cardSpriteRenderer;

    // Resource Management
    public ResourceManager resourceManager;
    public EndingManager endingManager;

    // Card Positioning and Movement
    public Vector2 defaultPositionCard;
    public float movingSpeed;
    public float sideMargin;
    public float sideTrigger;

    // Visual Properties
    public Color textColor;
    public float divideValue;

    // Game Flow
    public string Direction { get; private set; }
    private string leftQuote;
    private string rightQuote;
    public Card CurrentCard { get; private set; }
    public Flowchart flowchart;
    public Card testCard;
    public string nextCall;

    // Sound
    [SerializeField] private AudioSource backgroundMusicSource;

    // Pending Changes (Modifiers)
    public int pendingMoneyChange;
    public int pendingEnergyChange;
    public int pendingReputationChange;

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
    }

    void Start()
    {
        defaultPositionCard = new Vector2(0, cardGameObject.transform.position.y);
        cardGameObject.transform.position = defaultPositionCard;
        LoadCard(testCard);
        ResetCardToDefault();
        ResetVariablesToDefault();
    }

    void Update()
    {
        UpdateDialogue();
        CheckForEndings();
    }


    private void UpdateDialogue()
    {
        float swipeTextChangeThreshold = 2f; 

        float distanceFromCenter = Mathf.Abs(cardGameObject.transform.position.x - defaultPositionCard.x);

        if (distanceFromCenter <= swipeTextChangeThreshold)
        {
            actionQuote.text = flowchart.GetStringVariable("CharacterDialogue");
        }
        else
        {
            textColor.a = Mathf.Min((distanceFromCenter - sideMargin) / divideValue, 1);
            actionQuote.color = textColor;

            if (cardGameObject.transform.position.x < 0)
            {
                actionQuote.text = "Continue";
            }
            else
            {
                actionQuote.text = "Continue";
            }
        }
    }


    public void CheckForEndings() {
        if (MoneyStatus <= 0 && OnMoneyZero != null) {
            OnMoneyZero.Invoke();
        }
        else if (EnergyStatus <= 0 && OnEnergyZero != null) {
            OnEnergyZero.Invoke();
        }
        else if (ReputationStatus <= 0 && OnReputationZero != null) {
            OnReputationZero.Invoke();
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
            StartCoroutine(AnimateCardResetAndLoadNew(swipedRight));
        }
        else
        {
            Debug.Log("Retaining current card due to unclear swipe.");
            ResetCardToDefault();
        }
    }

    private IEnumerator AnimateCardResetAndLoadNew(bool swipedRight)
    {
        cardSpriteRenderer.enabled = false;

        yield return new WaitForSeconds(0.2f);

        cardGameObject.transform.position = defaultPositionCard;
        cardGameObject.transform.rotation = Quaternion.identity;

        ApplyCardEffect(CurrentCard, swipedRight); 
        NewCard();
        
        UpdateStatsVariables();

        yield return new WaitForSeconds(0.2f); 

        cardSpriteRenderer.enabled = true;
    }

    private IEnumerator ResetCardToDefaultAnimated()
    {
        Vector3 startPosition = cardGameObject.transform.position;
        Vector3 endPosition = defaultPositionCard;
        float duration = 0.5f; 
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            cardGameObject.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cardGameObject.transform.position = endPosition;
        cardGameObject.transform.rotation = Quaternion.identity;
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
        pendingMoneyChange = int.Parse(flowchart.GetStringVariable("ChangeMoney"));
        pendingEnergyChange = int.Parse(flowchart.GetStringVariable("ChangeEnergy"));
        pendingReputationChange = int.Parse(flowchart.GetStringVariable("ChangeStatus"));

        CalculatePendingEffects(pendingMoneyChange, pendingEnergyChange, pendingReputationChange);
        
        MoneyNumber.text = MoneyStatus.ToString();
        ReputationNumber.text = ReputationStatus.ToString();
        EnergyNumber.text = EnergyStatus.ToString();
    }

    public void CalculatePendingEffects(int pendingMoneyChange, int pendingEnergyChange, int pendingReputationChange)
    {
        MoneyStatus = MoneyStatus + pendingMoneyChange;
        EnergyStatus = EnergyStatus + pendingEnergyChange;
        ReputationStatus = ReputationStatus + pendingReputationChange;
    }

    public void ResetVariablesToDefault()
{
    MoneyStatus = 50;
    EnergyStatus = 50;
    ReputationStatus = 50;
}

}