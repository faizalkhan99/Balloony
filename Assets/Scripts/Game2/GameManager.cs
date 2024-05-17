using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Touch? firstTouch = null; // Track the first touch
    private Touch? secondTouch = null; // Track the second touch
    private Vector3[] touchPositions = new Vector3[2];
    private Transform selectedBalloon = null; // Track the selected balloon

    private void Awake()
    {
        touchPositions = new Vector3[2];
    }

    void Update()
    {
        if (UIManager.Instance._isTouchWorking)
        {
            UpdateTouches();
        }
    }

    private void UpdateTouches()
    {
        firstTouch = null;
        secondTouch = null;

        // Iterate through all touches
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            if (i < 2)
            {
                touchPositions[i] = Camera.main.ScreenToWorldPoint(touch.position);

                RaycastHit2D hit = Physics2D.Raycast(touchPositions[i], Vector2.zero);
                if (hit.collider != null)
                {
                    Debug.Log(hit.collider.name);
                    HandleTouch(touch, i, hit.collider.transform);
                }
            }
        }

        // Handle end of touch
        if ((firstTouch.HasValue && firstTouch.Value.phase == TouchPhase.Ended) ||
            (secondTouch.HasValue && secondTouch.Value.phase == TouchPhase.Ended))
        {
            selectedBalloon = null;
        }
    }

    private void HandleTouch(Touch touch, int touchIndex, Transform hitTransform)
    {
        if (touchIndex == 0)
        {
            if (!firstTouch.HasValue && (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved))
            {
                firstTouch = touch;
                SelectBalloon(hitTransform);
            }
            if (firstTouch.HasValue && selectedBalloon != null)
            {
                MoveObject(selectedBalloon, touchPositions[0]);
            }
        }
        else if (touchIndex == 1)
        {
            if (!secondTouch.HasValue && (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved))
            {
                secondTouch = touch;
                SelectBalloon(hitTransform);
            }
            if (secondTouch.HasValue && selectedBalloon != null)
            {
                MoveObject(selectedBalloon, touchPositions[1]);
            }
        }
    }

    private void SelectBalloon(Transform balloon)
    {
        selectedBalloon = balloon;
    }

    private Vector3 initialTouchPosition;
    private void MoveObject(Transform balloon, Vector3 touchPosition)
    {
        if (firstTouch.HasValue && firstTouch.Value.phase == TouchPhase.Began)
        {
            initialTouchPosition = balloon.position;
        }
        Vector3 direction = touchPosition - initialTouchPosition;
        balloon.position = initialTouchPosition + direction;
    }
}
