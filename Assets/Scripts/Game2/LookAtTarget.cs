using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _offset;
    void Update()
    {
        if (_target)
        {
            Vector2 newvect = (_target.position - transform.position).normalized;
            float angle = Mathf.Atan2(newvect.x, newvect.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, -angle + _offset);
        }
    }
}