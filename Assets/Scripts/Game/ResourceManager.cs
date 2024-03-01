using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public Sprite[] sprites;
    public Card[] cards;

    public int endingMoney;
    public int endingEnergy;
    public int endingReputation;

    void Start() {
        foreach (Card card in cards) {
            card.endingMoney = endingMoney;
            card.endingEnergy = endingEnergy;
            card.endingReputation = endingReputation;
        }
    }
}
