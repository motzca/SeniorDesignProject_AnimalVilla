using System.Collections;
using UnityEngine;

public class CardGravity : MonoBehaviour
{
    [SerializeField] private float swingSpeed = 5f;
    private Vector3 defaultPosition;
    private Vector3 touchStartPosition;
    private bool isCardDragging = false;
    private bool uiLocked = false;

    private void Start()
    {
        defaultPosition = transform.position;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    Vector3 touchPosBegin = Camera.main.ScreenToWorldPoint(touch.position);
                    touchPosBegin.z = 0f;
                    if (GetComponent<Collider2D>().OverlapPoint(touchPosBegin))
                    {
                        OnTouchBegin(touch.position);
                    }
                    break;
                case TouchPhase.Moved:
                    if (isCardDragging)
                    {
                        OnTouchMove(touch.position);
                    }
                    break;
                case TouchPhase.Ended:
                    if (isCardDragging)
                    {
                        OnTouchEnd();
                    }
                    break;
            }
        }

        if (!isCardDragging && !uiLocked)
        {
            ReturnToDefaultPosition();
        }
    }

    private void OnTouchBegin(Vector2 touchPosition)
    {
        // Haptic feedback
        #if UNITY_ANDROID || UNITY_IOS
        Handheld.Vibrate();
        #endif

        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.8f);

        isCardDragging = true;
        touchStartPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, 10f));
        touchStartPosition.z = 0f;

        uiLocked = true;
    }

    private void OnTouchMove(Vector2 touchPosition)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, 10f));
        transform.position = new Vector3(worldPosition.x, worldPosition.y, defaultPosition.z);
    }

    private void OnTouchEnd()
    {
        isCardDragging = false;

        GetComponent<SpriteRenderer>().color = Color.white;

        uiLocked = false;

        Vector3 swipeDirection = transform.position - touchStartPosition;
        float horizontalDistance = Mathf.Abs(swipeDirection.x);
        float verticalDistance = Mathf.Abs(swipeDirection.y);

        if (horizontalDistance > verticalDistance && horizontalDistance > 0.5f)
        {
            bool swipedRight = swipeDirection.x > 0;
            
            ApplyCardEffect(swipedRight);
        }
        else
        {
            StartCoroutine(AnimateCardToPosition(defaultPosition));
        }
    }


    private void ApplyCardEffect(bool swipedRight)
    {
        Debug.Log(swipedRight ? "Swiped Right" : "Swiped Left");
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

    private void ReturnToDefaultPosition()
    {
        transform.position = Vector3.Lerp(transform.position, defaultPosition, Time.deltaTime * swingSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * swingSpeed);
    }
}
