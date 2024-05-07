using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private CircleCollider2D circleCollider;
    private Touch? firstTouch = null; // Track the first touch
    private Touch? secondTouch = null; // Track the second touch

    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        // Reset touches at the beginning of each frame
        firstTouch = null;
        secondTouch = null;
        if (UIManager.Instance._isTouchWorking)
        {


            // Iterate through all touches
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

                // Check if this touch hits the circle collider
                if (Physics2D.OverlapPoint(touchPosition) == circleCollider)
                {
                    // Check if it's the first touch
                    if (!firstTouch.HasValue)
                    {
                        firstTouch = touch;
                    }
                    // Check if it's the second touch
                    else if (!secondTouch.HasValue)
                    {
                        secondTouch = touch;
                    }
                }
            }
            // Handle movement based on the first and second touches
            if (firstTouch.HasValue && (firstTouch.Value.phase == TouchPhase.Began || firstTouch.Value.phase == TouchPhase.Moved))
            {
                MoveObject(firstTouch.Value.position);
            }
            if (secondTouch.HasValue && (secondTouch.Value.phase == TouchPhase.Began || secondTouch.Value.phase == TouchPhase.Moved))
            {
                MoveObject(secondTouch.Value.position);
            }
        }
    }

    void MoveObject(Vector2 screenPosition)
    {
        // Move the object based on the position of the touch
        Vector2 touchPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        transform.position = touchPosition;
    }
}
