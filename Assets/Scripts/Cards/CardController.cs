using UnityEngine;

public class CardController : MonoBehaviour
{
    public Card card;
    private BoxCollider2D thisCard;
    private bool isDragging;
    private Vector3 dragStartPosition;

    private void Start()
    {
        thisCard = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        HandleTouchInput();
        HandleMouseInput();
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (thisCard.OverlapPoint(touchPos))
                    {
                        isDragging = true;
                    }
                    break;
                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        transform.position = new Vector3(touchPos.x, touchPos.y, transform.position.z);
                    }
                    break;
                case TouchPhase.Ended:
                    if (isDragging)
                    {
                        isDragging = false;
                    }
                    break;
            }
        }
    }

    private void HandleMouseInput()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (thisCard.OverlapPoint(mousePos))
            {
                isDragging = true;
                dragStartPosition = transform.position; // Record start position for swipe detection
            }
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            Vector3 dragEndPosition = transform.position; // Current position as the drag ends
            Vector3 swipeDirection = dragEndPosition - dragStartPosition;
            float horizontalDistance = Mathf.Abs(swipeDirection.x);
            float verticalDistance = Mathf.Abs(swipeDirection.y);

            if (horizontalDistance > verticalDistance && horizontalDistance > 0.5f) // Adjust 0.5f threshold as needed
            {
                bool swipedRight = swipeDirection.x > 0;
                Debug.Log(swipedRight ? "Swiped Right" : "Swiped Left");
            }
            else
            {
                Debug.Log("Swipe was unclear or too short.");
            }
        }
    }
}