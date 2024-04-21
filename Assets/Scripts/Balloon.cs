using UnityEngine;

public class Balloon : MonoBehaviour
{
    [SerializeField] private int _speed;
    private CircleCollider2D _balloonCollider;

    [SerializeField] private string _triggerName;
    private Animator _animator;
    public enum ObjectType{
        balloon,
        obstacle
    }
    public ObjectType type;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        if (_balloonCollider == null)
        {
            _balloonCollider = GetComponent<CircleCollider2D>();
        }
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
                Collider2D touchedCollider = Physics2D.OverlapCircle(touchPos, 1.75f);
                if (type == ObjectType.balloon)
                {

                    if (touchedCollider != null && touchedCollider == _balloonCollider && touchedCollider.CompareTag("Balloon"))
                    {
                        PlayPopSFX();
                        UIManager.Instance.UpdateScore();
                        DestroyBalloon();
                    }
                }
                else if (type == ObjectType.obstacle)
                {
                    if (touchedCollider != null && touchedCollider == _balloonCollider  && touchedCollider.CompareTag("Obstacle"))
                    {
                        PlayPopSFX();
                        UIManager.Instance.GameOverScreen();
                        DestroyBalloon();
                    }
                    Destroy(this.gameObject, 20f);
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
    [SerializeField] private AudioClip _popSFX;
    private void PlayPopSFX()
    {
        if(_popSFX && AudioManager.Instance)
        AudioManager.Instance.PlaySFX(_popSFX);
    }
}
