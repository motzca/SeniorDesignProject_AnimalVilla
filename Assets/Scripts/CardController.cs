using UnityEngine;

public class CardController : MonoBehaviour
{
    public Card card;
    private BoxCollider2D thisCard;
    private bool isMouseOver;

    private void Start()
    {
        thisCard = GetComponent<BoxCollider2D>();
    }

    private void OnMouseOver()
    {
        isMouseOver = true;
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
    }
}
