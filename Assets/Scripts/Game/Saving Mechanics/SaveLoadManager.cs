using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static void SaveGame(GameManager gameManager)
    {
        SaveData data = new SaveData
        {
            MoneyStatus = GameManager.MoneyStatus,
            EnergyStatus = GameManager.EnergyStatus,
            ReputationStatus = GameManager.ReputationStatus,
            CurrentCardId = GameManager.Instance.CurrentCard.cardId
        };

        string jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("SaveData", jsonData);
        PlayerPrefs.Save();
    }

    public static void LoadGame(GameManager gameManager)
    {
        if (PlayerPrefs.HasKey("SaveData"))
        {
            string jsonData = PlayerPrefs.GetString("SaveData");
            SaveData data = JsonUtility.FromJson<SaveData>(jsonData);

            GameManager.MoneyStatus = data.MoneyStatus;
            GameManager.EnergyStatus = data.EnergyStatus;
            GameManager.ReputationStatus = data.ReputationStatus;

            foreach (var card in GameManager.Instance.resourceManager.cards)
            {
                if (card.cardId == data.CurrentCardId)
                {
                    GameManager.Instance.LoadCard(card);
                    break;
                }
            }
        }
    }
}
