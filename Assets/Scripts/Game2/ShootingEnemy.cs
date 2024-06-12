using System.Collections;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _spawnPos;
    [SerializeField] private float _shootInterval;
    [SerializeField] private int _projectileSpeed;
    private void Start()
    {
        StartCoroutine(Shoot());
    }
    private IEnumerator Shoot()
    {
        while (gameObject)
        {
            yield return new WaitForSeconds(_shootInterval);

            Vector3 direction = (_target.position - _spawnPos.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            GameObject projectile = Instantiate(projectilePrefab, _spawnPos.position, Quaternion.Euler(new Vector3(0, 0, angle)));
            EnemyList.Instance.AddCloneToList(projectile);
            if (angle < 90 && angle >= -90)
            {
                projectile.transform.localScale = new Vector2(projectile.transform.localScale.x, -projectile.transform.localScale.y);    
            }
            projectile.GetComponent<Rigidbody2D>().AddForce(direction * _projectileSpeed, ForceMode2D.Impulse);
        }
    }
}