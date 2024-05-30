using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Dictionary<int, Transform> activeTouches = new Dictionary<int, Transform>(); // Track active touches and their associated balloons
    private Dictionary<int, Vector3> initialTouchPositions = new Dictionary<int, Vector3>(); // Track initial touch positions

    private void Update()
    {
        if (UIManager.Instance._isTouchWorking)
        {
            UpdateTouches();
        }

        foreach (var touchId in activeTouches.Keys)
        {
            if (Input.touchCount > touchId)
            {
                Touch touch = Input.GetTouch(touchId);
                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    Vector3 currentTouchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    currentTouchPosition.z = 0; // Assuming z = 0 for 2D
                    MoveObject(activeTouches[touchId], currentTouchPosition);
                }
            }
        }
    }

    private void UpdateTouches()
    {
        // Iterate through all touches
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0; // Assuming z = 0 for 2D

            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);
                if (hit.collider != null && !activeTouches.ContainsKey(touch.fingerId))
                {
                    SelectBalloon(touch.fingerId, hit.collider.transform);
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                if (activeTouches.ContainsKey(touch.fingerId))
                {
                    activeTouches.Remove(touch.fingerId);
                    initialTouchPositions.Remove(touch.fingerId);
                }
            }
        }
    }

    private void SelectBalloon(int touchId, Transform balloon)
    {
        activeTouches[touchId] = balloon;
        initialTouchPositions[touchId] = Camera.main.ScreenToWorldPoint(Input.GetTouch(touchId).position);
        //initialTouchPositions[touchId].z = 0; // Assuming z = 0 for 2D
    }
    [SerializeField] private byte _interpolationSpeed;
    private void MoveObject(Transform balloon, Vector3 touchPosition)
    {
        // Using Lerp to smooth the movement
        balloon.position = Vector3.Lerp(balloon.position, touchPosition, Time.deltaTime * _interpolationSpeed);
    }
}
