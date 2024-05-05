using UnityEngine;
public class Movement : MonoBehaviour
{
    [SerializeField] private float _minX;
    [SerializeField] private float _minY;
    [SerializeField] private float _maxX;
    [SerializeField] private float _maxY;
    [SerializeField] private float _speed;
    [SerializeField] private Vector2 _targetPosition;
    void Start()
    {
        _targetPosition = GetRandomPosition();
    }
    Vector2 GetRandomPosition()
    {
        float randomX = Random.Range(_minX, _maxX);
        float randomY = Random.Range(_minY, _maxY);
        return new Vector2(randomX, randomY);
    }
    void Update()
    {
        if((Vector2)transform.position != _targetPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);
        }
        else
        {
            _targetPosition = GetRandomPosition();
        }
    }
}