using UnityEngine;

public class CardEffectHandler : MonoBehaviour
{

    public void HandleSwipe(Card card, bool swipedRight)
    {
        if (swipedRight)
        {
            ApplyEffects(card.moneyStatRight, card.energyStatRight, card.reputationStatRight);
            Debug.Log($"{card.cardName} swiped right");
        }
        else
        {
            ApplyEffects(card.moneyStatLeft, card.energyStatLeft, card.reputationStatLeft);
            Debug.Log($"{card.cardName} swiped left");
        }
    }

    private void ApplyEffects(int moneyChange, int energyChange, int reputationChange)
    {
        GameManager.Instance.UpdateMoney(moneyChange);
        GameManager.Instance.UpdateEnergy(energyChange);
        GameManager.Instance.UpdateReputation(reputationChange);
    }
}
