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

    private SpriteRenderer frontSpriteRenderer; 

private void Start()
{
    thisCard = GetComponent<BoxCollider2D>();
    cardGravity = GetComponent<CardGravity>();
    frontSpriteRenderer = GetComponent<SpriteRenderer>();
}



    private void Update()
    {
        HandleTouchInput();
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

    private void ProcessSwipeEnd()
    {
        Vector3 swipeDirection = lastPosition - dragStartPosition;
        float horizontalDistance = Mathf.Abs(swipeDirection.x);
        float verticalDistance = Mathf.Abs(swipeDirection.y);

        float minSwipeDistance = 2.5f; 

        if (horizontalDistance > verticalDistance && horizontalDistance > minSwipeDistance)
        {
            bool swipedRight = swipeDirection.x > 0;
            GameManager.Instance.ProcessSwipeResult(swipedRight, true);
            StartCoroutine(FlipCardToShowFront());
        }
        else
        {
            Debug.Log("Swipe was unclear or not horizontal enough. No card change.");
            StartCoroutine(ResetCardPosition());
        }
    }

    private IEnumerator ResetCardPosition()
    {
        yield return AnimateCardToPosition(dragStartPosition);
    }

    private IEnumerator AnimateCardToPosition(Vector3 targetPosition)
    {
        float elapsedTime = 0;
        float totalTime = 0.5f;
        Vector3 startPosition = transform.position;
        float startAlpha = frontSpriteRenderer.color.a; 

        while (elapsedTime < totalTime)
        {
            float progress = elapsedTime / totalTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, progress);

            float alpha = Mathf.Lerp(startAlpha, 1f, progress); 
            frontSpriteRenderer.color = new Color(1f, 1f, 1f, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        frontSpriteRenderer.color = Color.white;
    }

    private IEnumerator LiftAndFlipCard()
    {
        Vector3 liftedPosition = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        yield return StartCoroutine(AnimateCardToPosition(liftedPosition, 0.1f));

        yield return StartCoroutine(FlipCardToShowFront());
    }

    private IEnumerator AnimateCardToPosition(Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0;
        Vector3 startPosition = transform.position;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }

    private IEnumerator FlipCardToShowFront()
    {
        transform.position = dragStartPosition;

        float flipDuration = 0.5f;
        Vector3 originalScale = transform.localScale; 

        frontSpriteRenderer.enabled = false; 

        for (float t = 0; t <= 0.5f; t += Time.deltaTime / (flipDuration / 2))
        {
            transform.localScale = new Vector3(Mathf.Lerp(originalScale.x, 0, t), originalScale.y, originalScale.z);
            yield return null;
        }

        for (float t = 0; t <= 0.5f; t += Time.deltaTime / (flipDuration / 2))
        {
            transform.localScale = new Vector3(Mathf.Lerp(0, originalScale.x, t), originalScale.y, originalScale.z);
            if (t > 0.25f) 
            {
                frontSpriteRenderer.enabled = true;
            }
            yield return null;
        }

        transform.localScale = originalScale;
    }

}