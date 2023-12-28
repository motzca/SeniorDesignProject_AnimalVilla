using UnityEngine;

public class DialogStarter : MonoBehaviour
{
    [SerializeField] private DialogBehaviour dialogBehaviour;
    [SerializeField] private DialogNodeGraph dialogGraph;
    [SerializeField] private GameManager gameManager; // Reference to GameManager
    private DialogNode currentNode; // Variable to store the current node

    private void Start()
    {
        dialogBehaviour.StartDialog(dialogGraph);
        dialogBehaviour.OnNodeComplete += OnNodeComplete; // Subscribe to the node complete event
        dialogBehaviour.OnChoiceSelected += OnChoiceSelected; // Subscribe to the choice event
    }

    private void OnNodeComplete(DialogNode node)
    {
        currentNode = node; // Update the current node
        // Additional logic you may want to perform when a node is complete
    }

    private void OnChoiceSelected(DialogChoice choice)
    {
        // Handle the choice made by the player using the current node
        gameManager.HandleChoice(currentNode, choice);
    }
}
