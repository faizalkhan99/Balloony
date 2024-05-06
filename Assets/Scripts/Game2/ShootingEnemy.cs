using System.Collections;
using System.Collections.Generic;
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
        InvokeRepeating(nameof(Shoot), _shootInterval, _shootInterval);
    }

    private void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, _spaenPos.position, Quaternion.identity);
        Vector3 direction = (_target.position - transform.position).normalized;
        projectile.GetComponent<Rigidbody2D>().AddForce((_projectileSpeed * direction), ForceMode2D.Impulse);
        projectile.transform.up = direction;
    }
}
