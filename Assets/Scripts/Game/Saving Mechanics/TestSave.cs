using UnityEngine;

public class TestSave : MonoBehaviour
{
   void Start()
    {
        // Trigger saving the game state
        SaveLoadManager.SaveGame(GameManager.Instance);

        // Output a message to indicate that the game state has been saved
        Debug.Log("Game state saved.");

        // Load the game state
        SaveLoadManager.LoadGame(GameManager.Instance);

        // Output a message to indicate that the game state has been loaded
        Debug.Log("Game state loaded.");
    }
}
