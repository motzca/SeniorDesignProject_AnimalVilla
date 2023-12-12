using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogNode
{
    public Card card;
    // Add any additional properties needed for your dialog node
}

public class DialogSystem : MonoBehaviour
{
    public List<DialogNode> dialogNodes;
    private int currentNodeIndex = 0;

    // Add any other properties or methods needed for your dialog system

    public DialogNode GetNextNode()
    {
        // Check if there are more dialog nodes
        if (currentNodeIndex < dialogNodes.Count)
        {
            // Get the next dialog node
            DialogNode nextNode = dialogNodes[currentNodeIndex];
            currentNodeIndex++;

            return nextNode;
        }
        else
        {
            // Return null when there are no more dialog nodes
            return null;
        }
    }
}

