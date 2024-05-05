using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float _offset;
    void Update()
    {
        Vector2 newvect = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(newvect.x, newvect.y)* Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,-angle + _offset);
    }
}
