using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Transform _targetPos;
    void Update()
    {
        transform.position += Vector3.MoveTowards(transform.position, _targetPos.position, .5f);
    }
}
