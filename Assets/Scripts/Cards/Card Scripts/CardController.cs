using UnityEngine;
using System.Collections;


public class CardController : MonoBehaviour
{
    public Card card;
    private BoxCollider2D thisCard;
    private bool isDragging;
    private Vector3 dragStartPosition; 
    private Vector3 lastPosition;
    private CardGravity cardGravity;

    private void Start()
    {
        thisCard = GetComponent<BoxCollider2D>();
        cardGravity = GetComponent<CardGravity>();
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
                        dragStartPosition = transform.position;
                    }
                    break;
                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        transform.position = new Vector3(touchPos.x, touchPos.y, transform.position.z);
                        lastPosition = transform.position;
                    }
                    break;
                case TouchPhase.Ended:
                    if (isDragging)
                    {
                        isDragging = false;
                        ProcessSwipeEnd();
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
                dragStartPosition = transform.position; 
            }
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
            lastPosition = transform.position;
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            ProcessSwipeEnd(); 
        }
    }

    private void ProcessSwipeEnd()
    {
        Vector3 swipeDirection = lastPosition - dragStartPosition;
        float horizontalDistance = Mathf.Abs(swipeDirection.x);
        float verticalDistance = Mathf.Abs(swipeDirection.y);

        if (horizontalDistance > verticalDistance && horizontalDistance > 1.0f) 
        {
            bool swipedRight = swipeDirection.x > 0;
            GameManager.Instance.ProcessSwipeResult(swipedRight, true);
        }
        else
        {
            Debug.Log("Swipe was unclear or not horizontal enough. No card change.");
            GameManager.Instance.ProcessSwipeResult(false, false);
        }
    }

    private IEnumerator AnimateCardToPosition(Vector3 targetPosition)
    {
        float elapsedTime = 0;
        float totalTime = 0.5f;
        Vector3 startPosition = transform.position;

        while (elapsedTime < totalTime)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / totalTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }
}
