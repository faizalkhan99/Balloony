/*using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField] private Transform[] _target;
    [SerializeField] private float _offset;
    void Update()
    {
        if (_target != null)
        {
            int randTarget = Random.Range(0, _target.Length);
            Vector2 newvect = (_target[randTarget].position - transform.position).normalized;
            float angle = Mathf.Atan2(newvect.x, newvect.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, -angle + _offset);
        }
    }
}*/
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField] private Transform[] _targets; 
    [SerializeField] private float _offset;
    [SerializeField] private float _rotationSpeed = 5f;
    private Transform _currentTarget;
    private Quaternion _targetRotation;

    [SerializeField] private ShootingEnemy _shootingEnemy;

    private void Start()
    {
        if (_shootingEnemy != null)
        {
            // Subscribe to the OnShoot event
            _shootingEnemy.OnShoot.AddListener(HandleShoot);
        }
    }

    private void LateUpdate()
    {
        if (_currentTarget != null)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);
        }
        CalculateTargetRotation();
    }

    public void HandleShoot()
    {
        if (_targets != null && _targets.Length > 0)
        {
            int randTargetIndex = Random.Range(0, _targets.Length);
            _currentTarget = _targets[randTargetIndex];
            
        }
    }
    

    private void CalculateTargetRotation()
    {
        if (_currentTarget != null)
        {
            Vector2 direction = (_currentTarget.position - transform.position).normalized;

            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

            _targetRotation = Quaternion.Euler(0, 0, -angle + _offset);
        }
    }
}
