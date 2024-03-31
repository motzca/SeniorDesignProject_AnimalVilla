using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public Sprite[] sprites;
    public Card[] cards;

    public Sprite GetSpriteByName(string spriteName)
    {
        foreach (Sprite sprite in sprites)
        {
            if (sprite.name == spriteName)
            {
                return sprite;
            }
        }
        return null;
    }
}