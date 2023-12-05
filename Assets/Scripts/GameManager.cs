using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Game objects
    public GameObject cardGameObject;
    public CardController mainCardController;
    public SpriteRenderer cardSpriteRenderer;
    public ResourceManager resourceManager;

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
        textColor.a = Mathf.Min((Mathf.Abs(cardGameObject.transform.position.x) - fSideMargin)/ divideValue, 1);
        if (cardGameObject.transform.position.x > fSideTrigger)
        {
            if (Input.GetMouseButtonUp(0))
            {
              currentCard.Right();
                NewCard();
            }
            
        } 
        else if (cardGameObject.transform.position.x > -fSideMargin)
        {
            textColor.a = 0;
        }
        else
        {
            if (Input.GetMouseButtonUp(0))
            {
                currentCard.Left();
                NewCard();
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
            cardGameObject.transform.position = Vector2.MoveTowards(cardGameObject.transform.position, new Vector2(0,0), fMovingSpeed);
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
        int rollDice = Random.Range(0, resourceManager.cards.Length + 1);
        LoadCard(resourceManager.cards[rollDice]);
    }
}
