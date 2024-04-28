using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private CircleCollider2D circleCollider;
    [SerializeField] private bool _moveAllowed;
    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
    }

 
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchposition = Camera.main.ScreenToWorldPoint(touch.position);

            if(touch.phase == TouchPhase.Began)
            {
                Collider2D touchedCollider = Physics2D.OverlapPoint(touchposition);
                if(circleCollider == touchedCollider)
                {
                    _moveAllowed = true;
                }
            }
            if(touch.phase == TouchPhase.Moved)
            {
                if(_moveAllowed)
                {
                    transform.position = new Vector2(touchposition.x, touchposition.y);
                }
            }
            if(touch.phase == TouchPhase.Ended)
            {
                _moveAllowed = false;
            }
        }
    }
}
