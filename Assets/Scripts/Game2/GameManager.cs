using UnityEngine;
public class GameManager : MonoBehaviour
{
    private Touch? firstTouch = null; // Track the first touch
    private Touch? secondTouch = null; // Track the second touch
    private Vector3[] touchPosition;
    private Transform selectedBalloon = null; // Track the selected balloon
    private Rigidbody2D selectedBalloonRigidbody; // Track the Rigidbody component of the selected balloon
    private void Awake()
    {
        touchPosition = new Vector3[2];
    }
    void Update()
    {
        firstTouch = null;
        secondTouch = null;
        touchPosition = new Vector3[2];
        if (UIManager.Instance._isTouchWorking)
        {
            // Iterate through all touches
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                touchPosition[i] = Camera.main.ScreenToWorldPoint(touch.position);

                RaycastHit2D hit = Physics2D.Raycast(touchPosition[i], Vector2.zero);
                if (hit.collider != null)
                {
                    Debug.Log(hit.collider.name);
                    if (i == 0)
                    {
                        if (!firstTouch.HasValue && (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved))
                        {
                            firstTouch = touch;
                            selectedBalloon = hit.collider.transform; // Select the balloon
                            if (selectedBalloon.TryGetComponent<Rigidbody2D>(out selectedBalloonRigidbody))
                            {
                                selectedBalloonRigidbody.simulated = true; // Disable Rigidbody
                            }
                        }
                        if (firstTouch.HasValue && selectedBalloon != null)
                        {
                            MoveObject(selectedBalloon, touchPosition[i]);
                        }
                    }
                    else if (i == 1)
                    {
                        if (!secondTouch.HasValue && (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved))
                        {
                            secondTouch = touch;
                            selectedBalloon = hit.collider.transform; // Select the balloon
                            if (selectedBalloon.TryGetComponent<Rigidbody2D>(out selectedBalloonRigidbody))
                            {
                                selectedBalloonRigidbody.simulated = false; // Disable Rigidbody
                            }
                        }
                        if (secondTouch.HasValue && selectedBalloon != null)
                        {
                            MoveObject(selectedBalloon, touchPosition[i]);
                        }
                    }
                }
            }
            if ((firstTouch.HasValue && firstTouch.Value.phase == TouchPhase.Ended) ||
                (secondTouch.HasValue && secondTouch.Value.phase == TouchPhase.Ended))
            {
                selectedBalloon = null;
                if (selectedBalloonRigidbody != null)
                {
                    selectedBalloonRigidbody.simulated = true; // Enable Rigidbody
                }
            }
        }
    }
    void MoveObject2(Transform balloon, Vector3 touchPosition)
    {
        balloon.position = touchPosition;
    }
    private Vector3 initialTouchPosition;
    private void MoveObject(Transform balloon, Vector3 touchPosition)
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            initialTouchPosition = touchPosition;
        }
        Vector3 direction = touchPosition - initialTouchPosition;
        balloon.position = initialTouchPosition + direction;
    }
}