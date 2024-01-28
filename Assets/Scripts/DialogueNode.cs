using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class DialogueNode : ScriptableObject
{
    public int CardId;
    public string Direction;
    public Card Card;
}

