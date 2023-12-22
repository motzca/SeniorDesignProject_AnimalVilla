using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public DialogSystem dialogSystem;
    public GameManager gameManager;

    void Start()
    {
        dialogSystem.OnDialogCompleted += LoadNextDialogNode;
    }

    public void StartDialog()
    {
        // Start the dialog system and get the first node
        DialogNode initialNode = dialogSystem.GetNextNode();
        dialogSystem.OnDialogCompleted += LoadNextDialogNode;
        LoadDialogNode(initialNode);
    }

    void LoadDialogNode(DialogNode dialogNode)
    {
        DialogNode nextNode = dialogSystem.GetNextNode();
        if (nextNode != null)
        {
            LoadDialogNode(nextNode);
        }
        else
        {
            EndDialog();
        }
    }

    void EndDialog()
    {
        // Add logic to handle the end of the dialog
        // For example, you might close the dialog UI, update game states, etc.
        Debug.Log("End of dialog");
    }
}

