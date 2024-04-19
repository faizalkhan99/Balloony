using UnityEngine;

public class Balloon : MonoBehaviour
{
    [SerializeField] private float _speed;
    private CircleCollider2D _balloonCollider;


    void Start()
    {
        if (_balloonCollider == null)
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

            if (touch.phase == TouchPhase.Began)
            {
                Debug.Log("Touch Taken");
                Collider2D touchedCollider = Physics2D.OverlapCircle(touchPos, 1.75f);
                if (touchedCollider != null && touchedCollider == _balloonCollider && touchedCollider.CompareTag("Balloon"))
                {
                    UIManager.Instance.UpdateScore();
                    PlayPopSFX();
                    DestroyBalloon();
                }
            }
        }
    }

    private void DestroyBalloon()
    {
        Destroy(this.gameObject);
    }
    private void PlayPopSFX()
    {

    }




    
}
