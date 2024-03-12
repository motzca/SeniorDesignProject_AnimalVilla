using UnityEngine;

public class StatusMonitor : MonoBehaviour
{
    public CardData energyZeroCard;
    public CardData moneyZeroCard;
    public CardData reputationZeroCard;

    private ResourceManager resourceManager;

    void Start()
    {
        resourceManager = FindObjectOfType<ResourceManager>();
    }

    void Update()
    {
        if (GameManager.MoneyStatus <= 0)
        {
            LoadSpecialEndingCard(moneyZeroCard);
            return;
        }

        if (GameManager.EnergyStatus <= 0)
        {
            LoadSpecialEndingCard(energyZeroCard);
            return;
        }

        if (GameManager.ReputationStatus <= 0)
        {
            LoadSpecialEndingCard(reputationZeroCard);
            return;
        }
    }

    private void LoadSpecialEndingCard(CardData cardData)
    {
        Card card = FindCardFromCardData(cardData);


        if (card != null)
        {
            GameManager.Instance.LoadCard(card);
        }
        else
        {
            Debug.LogError($"Failed to find Card object for CardData: {cardData.cardName}");
        }
    }

    private Card FindCardFromCardData(CardData cardData)
    {
        if (cardData == null)
        {
            Debug.LogError("CardData object is null.");
            return null;
        }

        foreach (Card card in resourceManager.cards)
        {
            if (card.cardName == cardData.cardName)
            {
                return card;
            }
        }

        return null;
    }
}
