using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    [SerializeField] private float _spped;
    private CircleCollider2D _balloonCollider;
    void Start()
    {
        _balloonCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _spped * Time.deltaTime * Vector3.up;
        if (Input.touchCount > 0)
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
            }
        }
    }
}
