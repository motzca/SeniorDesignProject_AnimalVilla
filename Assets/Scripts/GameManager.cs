using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Stat variables
    public static int moneyStatus = 50;
    public static int energyStatus = 50;
    public static int reputationStatus = 50;
    public static int maxValue = 100;
    public static int minValue = 0;

    //Game objects
    public GameObject cardGameObject;
    public CardController mainCardController;
    public SpriteRenderer cardSpriteRenderer;
    public ResourceManager resourceManager;
    public Vector2 defaultPositionCard;
    public DialogSystem mainDialogSystem;

    // Tweaking variables
    public float fMovingSpeed;
    public float fSideMargin;
    public float fSideTrigger;
    Vector3 pos;
    float alphaText;
    public Color textColor;
    public float divideValue;

    // UI
    public TMP_Text characterDialogue;
    public TMP_Text actionQuote;

    // Card variables
    public string direction;
    private string leftQuote;
    private string rightQuote;
    public Card currentCard;
    public Card testCard;

    void Start()
    {
        LoadCard(testCard);
    }

    void UpdateDialogue()
    {
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

    void Update()
    {
        // Stat values logic


        // Dialogue text handling
        textColor.a = Mathf.Min((Mathf.Abs(cardGameObject.transform.position.x) - fSideMargin)/ divideValue, 1);
        if (cardGameObject.transform.position.x > fSideTrigger)
        {
            if (Input.GetMouseButtonUp(0))
            {
              currentCard.Right();
              NewCard();
              direction = "right";
            }
            
        } 
        else if (cardGameObject.transform.position.x > -fSideMargin)
        {
            direction = "none";
            textColor.a = 0;
        }
        else
        {
            if (Input.GetMouseButtonUp(0))
            {
                currentCard.Left();
                NewCard();
                direction = "left";
            }
        }
        UpdateDialogue();

        //Movement
        if (Input.GetMouseButton(0) && mainCardController.isMouseOver)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cardGameObject.transform.position = pos;
        }
        else
        {
            cardGameObject.transform.position = Vector2.MoveTowards(cardGameObject.transform.position, defaultPositionCard, fMovingSpeed);
        }
    }

    public void LoadCard(Card card)
    {
        cardSpriteRenderer.sprite = resourceManager.sprites[(int)card.sprite];
        leftQuote = card.leftQuote;
        rightQuote = card.rightQuote;
        currentCard = card;
        characterDialogue.text = card.dialogue;
    }

    public void NewCard()
    {
        DialogNode nextNode = mainDialogSystem.GetNextNode();

        // Check if there is a valid dialog node
        if (nextNode != null)
        {
            // Load the card based on the dialog node
            LoadCard(nextNode.card);
        }

    }
}
