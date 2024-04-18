using UnityEngine;

public class Balloon : MonoBehaviour
{
    [SerializeField] private float _speed;
    private CircleCollider2D _balloonCollider; 
    void Awake()
    {
        if (_balloonCollider != null)
        {
            _balloonCollider = GetComponent<CircleCollider2D>();
            Debug.Log("Colllider Found!)");
        }
        else Debug.Log("Colllider Not Found!)");
    }

    void Update()
    {
        transform.position += _speed * Time.deltaTime * Vector3.up;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

            RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);
            if(hit.collider != null)
            {
                Debug.Log("Raycast hit something: " +hit.collider.gameObject.name);
            }
            else
            {
                Debug.Log("Raycast did not hit anything: " + hit.collider.gameObject.name);
            }

            if (hit.collider != null && hit.collider == _balloonCollider)
            {
                Debug.Log("Balloon Popped!");
                // Play sound here and increase score
                Destroy(this.gameObject);
            }
            else
            {
                Debug.Log("Touch did not overlap with balloon collider");
            }
        }



        /*if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            if (touch.phase == TouchPhase.Began)
            {
                Collider2D touchedCollider = Physics2D.OverlapPoint(touchPos);
                if (touchedCollider == _balloonCollider)
                {
                    Debug.Log("Balloon Popped!");
                    //play sound here + increase score.
                    Destroy(this.gameObject);
                }
                else
                {
                    Debug.Log("Colllider Not Found!)");
                }
            }
        }*/
    }
}
