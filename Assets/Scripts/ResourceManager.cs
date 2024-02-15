using System.IO;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public Sprite[] sprites;
    public CardData[] cardData;

    private void Awake()
    {
        LoadCardDataFromJSON();
    }

    private void LoadCardDataFromJSON()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Scripts", "Cards", "CardHardData.json");

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            cardData = JsonUtility.FromJson<CardData[]>(dataAsJson);
        }
        else
        {
            Debug.LogError("Cannot find card data JSON file at path: " + filePath);
        }
    }
}
