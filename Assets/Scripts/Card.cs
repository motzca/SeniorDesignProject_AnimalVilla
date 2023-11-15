using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]

public class Card : ScriptableObject
{
    public CardSprite sprite;
    public string leftQuote;
    public string rightQuote;
    public string cardName;
    public int cardId;

    public void Left()
    {
        Debug.Log(cardName + "swipped left");
    }

    public void Right()
    {
        Debug.Log(cardName + "swipped right");
    }
}
