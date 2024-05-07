using System.Collections;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _spaenPos;
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
            GameObject projectile = Instantiate(projectilePrefab, _spaenPos.position, Quaternion.identity);
            EnemyList.Instance.AddCloneToList(projectile);
            Vector3 direction = (_target.position - transform.position).normalized;
            projectile.GetComponent<Rigidbody2D>().AddForce((_projectileSpeed * direction), ForceMode2D.Impulse);
            float velocity = projectile.GetComponent<Rigidbody2D>().velocity.x > 0 ? -1 : 1;
            projectile.transform.localScale = new Vector2(velocity * projectile.transform.localScale.x, projectile.transform.localScale.y);
            projectile.transform.up = direction;
        }
    }
}