using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static void SaveGame(GameManager gameManager) {
        SaveData data = new SaveData {
            MoneyStatus = gameManager.MoneyStatus;
            EnergyStatus = gameManager.EnergyStatus;
            ReputationStatus = gameManager.ReputationStatus;
            CurrentCardId = gameManager.instance.CurrentCard.cardId
        };

        string jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("SaveData", jsonData);
        PlayerPrefs.Save();
    }

    public static void LoadGame(GameManager gameManager){
        if(PlayerPrefs.HasKey("SaveData")) {
            string jsonData = PlayerPrefs.GetString("SaveData");
            SaveData data = JsonUtility.FromJson<SaveData>(jsonData);

            gameManager.MoneyStatus = data.MoneyStatus;
            gameManager.EnergyStatus = data.EnergyStatus;
            gameManager.ReputationStatus = data.ReputationStatus;

            foreach (var card in gameManager.resourceManager.cards) {
                if (card.cardId == data.CurrentCardId) {
                    gameManager.LoadCard(card);
                    break;
                }
            }
        }
    }
}
