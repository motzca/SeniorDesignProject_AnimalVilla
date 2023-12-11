using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogNode
{
    public Card card;
}

public class DialogSystem : MonoBehaviour
{
    public List<DialogNode> dialogNodes;
    private int currentNodeIndex = 0;

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
