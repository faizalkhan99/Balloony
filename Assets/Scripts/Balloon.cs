using UnityEngine;

public class Balloon : MonoBehaviour
{
    [SerializeField] private int _speed;
    private CircleCollider2D _balloonCollider;

    [SerializeField] private string _triggerName;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        if (_balloonCollider == null)
        {
            _balloonCollider = GetComponent<CircleCollider2D>();
            Debug.Log("Colllider Found!)");
        }
    }

    void Update()
    {
        transform.position += _speed * Time.deltaTime * Vector3.up;
        Debug.Log("Speed: " + _speed);
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Began)
            {
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
    public void SetBalloonSpeed(int speed)
    {
        _speed = speed ;
    }
    private void DestroyBalloon()
    {
        _animator.SetTrigger(_triggerName);
        _balloonCollider.enabled = false;
        Destroy(this.gameObject, 0.5f);
    }
    private void PlayPopSFX()
    {

    }
}
