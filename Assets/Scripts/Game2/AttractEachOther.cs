using UnityEngine;

public class AttractEachOther : MonoBehaviour
{
    [SerializeField] private Transform _targetPos;
    [SerializeField] private int _speed;
    private LineRenderer _lineRenderer;

    void Start()
    {
        if (!TryGetComponent<LineRenderer>(out _lineRenderer))
        {
            _lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        _lineRenderer.positionCount = 2;
        _lineRenderer.startWidth = 0.5f; 
        _lineRenderer.endWidth = 0.5f; 

        _lineRenderer.SetPosition(0, transform.position);
    }

    void Update()
    {
        Vector3 directionToTarget = _targetPos.position - transform.position;
        transform.position += _speed * Time.deltaTime * directionToTarget.normalized;
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, _targetPos.position);
    }
}