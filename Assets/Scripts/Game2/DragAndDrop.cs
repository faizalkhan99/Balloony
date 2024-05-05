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
        if (Input.touchCount > 2)
        {
            return;
        }
        if (UIManager.Instance._isTouchWorking)
        {


            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                Vector2 touchposition = Camera.main.ScreenToWorldPoint(touch.position);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        Collider2D touchedCollider = Physics2D.OverlapPoint(touchposition);
                        if (circleCollider == touchedCollider)
                        {
                            _moveAllowed = true;
                        }
                        break;

                    case TouchPhase.Moved:
                        if (_moveAllowed)
                        {
                            transform.position = new Vector2(touchposition.x, touchposition.y);
                        }
                        break;

                    case TouchPhase.Ended:
                        _moveAllowed = false;
                        break;
                }
            }
        }
        else
        {
            return;
        }
    }
}