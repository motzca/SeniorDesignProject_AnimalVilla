using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject cardGameObject;
    public CardController mainCardController;
    public SpriteRenderer cardSpriteRenderer;
    public float fMovingSpeed;
    public float fSideMargin;
    public float fSideTrigger;
    Vector3 pos;
    public TMP_Text display;
    public TMP_Text dialogue;
    float alphaText;
    public Color textColor;

    void Start()
    {
    }
    void Update()
    {
        //Dialogue text handling
        textColor.a = Mathf.Min(Mathf.Abs(cardGameObject.transform.position.x/2), 1);
        dialogue.color = textColor;

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

        //Checking right side
        if (cardGameObject.transform.position.x > fSideMargin)
        {
            // dialogue.alpha = Mathf.Min(cardGameObject.transform.position.x, 1);
            // dialouge.color.a = Mathf.Min(cardGameObject.transform.position.x, 1);
            if(!Input.GetMouseButton(0) && cardGameObject.transform.position.y > fSideTrigger)
            {
                Debug.Log("Going left");
            }
        }
        else if (cardGameObject.transform.position.x < -fSideMargin)
        {
            // dialogue.alpha = Mathf.Min(-cardGameObject.transform.position.x, 1);
            if (!Input.GetMouseButton(0) && cardGameObject.transform.position.y < -fSideTrigger)
            {
                Debug.Log("Going right");
            }
        }
        else
        {
            cardSpriteRenderer.color = Color.white;
        }

        // UI
        display.text = "" + textColor.a;
    }
}
