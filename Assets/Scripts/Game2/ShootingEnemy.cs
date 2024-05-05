using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab; 
    [SerializeField] private Transform target; 
    [SerializeField] private float shootInterval; 
    [SerializeField] private int _projectileSpeed;
    private void Start()
    {
        InvokeRepeating(nameof(Shoot), shootInterval, shootInterval);
    }

    private void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Vector3 direction = (target.position - transform.position).normalized;
        projectile.GetComponent<Rigidbody2D>().AddForce((_projectileSpeed * direction), ForceMode2D.Impulse);
        projectile.transform.up = direction;
    }
}
