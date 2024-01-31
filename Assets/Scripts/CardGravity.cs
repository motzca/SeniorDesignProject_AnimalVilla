using UnityEngine;

public class CardGravity : MonoBehaviour
{
    [SerializeField] private float swingSpeed = 5f;
    private Vector3 defaultPosition;
    private float zRotation;
    private bool isCardDragging = false;

    private void Start()
    {
        defaultPosition = transform.position;
    }

    private void Update()
    {
        if (!isCardDragging)
            ReturnToDefaultPosition();
    }

    private void OnMouseDown()
    {
        isCardDragging = true;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        zRotation = Mathf.Abs((mousePos.x - transform.position.x) * 6.5f);
        zRotation = transform.position.x <= 0 ? zRotation : zRotation * -1;
    }

    private void OnMouseDrag()
    {
        Vector3 desiredPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        transform.position = desiredPosition;
    }

    private void OnMouseUp()
    {
        isCardDragging = false;
    }

    private void ReturnToDefaultPosition()
    {
        transform.position = Vector3.Lerp(transform.position, defaultPosition, Time.deltaTime * swingSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * swingSpeed);
    }
}
