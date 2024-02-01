using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class DialogueNode : ScriptableObject
{
    public int CardId;
    public string Direction;
    public Card Card;

    public string SpeechText;
    public DialogueOption[] Options;
}

[System.Serializable]
public class DialogueOption
{
    public string Quote;
    public Card ResultCard;
    public string SpeechText;
}

