using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Touch? firstTouch = null; // Track the first touch
    private Touch? secondTouch = null; // Track the second touch
    private Vector3[] touchPosition;
    private Transform selectedBalloon = null; // Track the selected balloon

    private void Awake()
    {
        touchPosition = new Vector3[2];
    }

    void Update()
    {
        // Reset touches at the beginning of each frame
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
            }
        }
    }

    void MoveObject(Transform balloon, Vector3 touchPosition)
    {
        balloon.position = touchPosition;
    }
}