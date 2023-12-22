using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogNode
{
    public string sentenceText;
    public string leftQuote;
    public string rightQuote;
}

public class DialogSystem : MonoBehaviour
{
    public List<DialogNode> dialogNodes;
    private int currentNodeIndex = 0;

    public delegate void DialogCompletedEventHandler();
    public event DialogCompletedEventHandler OnDialogCompleted;

    // ... Existing code ...

    public void StartDialog()
    {
        // Reset the node index when starting a new dialog
        currentNodeIndex = 0;
        // Trigger the first node
        OnDialogCompleted?.Invoke();
    }

    // ... Existing code ...
}



