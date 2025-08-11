using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ShootingEnemy : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform[] _target;
    [SerializeField] private Transform _spawnPos;
    [SerializeField] private float _shootInterval;
    [SerializeField] private int _projectileSpeed;

    public UnityEvent OnShoot = new(); // Event to notify when shooting

    // Using OnEnable is better practice than Start for restarting coroutines.
    private void OnEnable()
    {
        StartCoroutine(Shoot());
    }

    public IEnumerator Shoot()
    {
        float initialDelay = Random.Range(0f, _shootInterval);
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            yield return new WaitForSeconds(_shootInterval);

            int randTarget = Random.Range(0, _target.Length);
            Vector3 direction = (_target[randTarget].position - _spawnPos.position).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            GameObject projectile = Instantiate(projectilePrefab, _spawnPos.position, Quaternion.Euler(new Vector3(0, 0, angle)));
            AudioManager.Instance.PlaySFX(SoundID.PaperWoosh);
            EnemyList.Instance.AddCloneToList(projectile);

            if (angle < 90 && angle >= -90)
            {
                projectile.transform.localScale = new Vector2(projectile.transform.localScale.x, -projectile.transform.localScale.y);
            }
            projectile.GetComponent<Rigidbody2D>().AddForce(direction * _projectileSpeed, ForceMode2D.Impulse);

            OnShoot?.Invoke();
        }
    }
}

/*
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ShootingEnemy : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform[] _target;
    [SerializeField] private Transform _spawnPos;
    [SerializeField] private float _shootInterval;
    [SerializeField] private int _projectileSpeed;

    public UnityEvent OnShoot = new(); // Event to notify when shooting

    private void Start()
    {
        StartCoroutine(Shoot());
    }

    public IEnumerator Shoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(_shootInterval);

            // Select a random target
            int randTarget = Random.Range(0, _target.Length);
            Vector3 direction = (_target[randTarget].position - _spawnPos.position).normalized;

            // Calculate angle for projectile rotation
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Instantiate and launch the projectile
            GameObject projectile = Instantiate(projectilePrefab, _spawnPos.position, Quaternion.Euler(new Vector3(0, 0, angle)));
            EnemyList.Instance.AddCloneToList(projectile);

            if (angle < 90 && angle >= -90)
            {
                projectile.transform.localScale = new Vector2(projectile.transform.localScale.x, -projectile.transform.localScale.y);
            }
            projectile.GetComponent<Rigidbody2D>().AddForce(direction * _projectileSpeed, ForceMode2D.Impulse);

            // Trigger the OnShoot event
            OnShoot?.Invoke();
        }
    }
}
*/