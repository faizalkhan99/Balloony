// using System.Collections;
// using UnityEngine;

// public class Spawner : MonoBehaviour
// {
//     [SerializeField] private GameObject[] _objstacle;

//     [SerializeField] private float _timeBtwSpawn;
//     [SerializeField] private float _startTimeBtwSpawn;
//     [SerializeField] private float _decreaseTime;
//     [SerializeField] private float _minTime;

//     [SerializeField] private float _SpawnRangeMinX;
//     [SerializeField] private float _SpawnRangeMaxX;

//     [SerializeField] private int _balloonSpeed;

    
//     private void Start()
//     {
//         StartCoroutine(IncreaseSpeed());
//     }
//     void Update()
//     {
//         if (_timeBtwSpawn <= 0)
//         {
//             Vector3 randPos = new(Random.Range(_SpawnRangeMinX, _SpawnRangeMaxX), -26, 0);
//             GameObject balloon = Instantiate(_objstacle[Random.Range(0, _objstacle.Length)], randPos, Quaternion.identity);
//             if (balloon != null)
//             {
//                 balloon.GetComponent<Balloon>().SetBalloonSpeed(_balloonSpeed);
//             }
//             _timeBtwSpawn = _startTimeBtwSpawn;
            
//         }
//         else
//         {
//             _timeBtwSpawn -= Time.deltaTime;
//         }
//     }
//     private IEnumerator IncreaseSpeed()
//     {
//         while (true)
//         {
//             yield return new WaitForSeconds(10f);
//             if (_balloonSpeed <= 100)
//             {
//                 _balloonSpeed += 5;
//             }
//             if (_startTimeBtwSpawn > _minTime)
//             {
//                 _startTimeBtwSpawn -= _decreaseTime;

//             }
//         }
//     }

// }










































using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Spawner : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private GameObject[] _obstacle;
    
    [Header("Timing")]
    [SerializeField] private float _startTimeBtwSpawn;
    [SerializeField] private float _decreaseTime;
    [SerializeField] private float _minTime;
    
    [Header("Movement")]
    [SerializeField] private int _balloonSpeed;

    private float _timeBtwSpawn;
    private BoxCollider2D _spawnArea;

    private void Awake()
    {
        _spawnArea = GetComponent<BoxCollider2D>();
        _spawnArea.isTrigger = true; // Ensure it doesn't bump into things
        
        FitColliderToScreenWidth();
    }

    private void Start()
    {
        _timeBtwSpawn = _startTimeBtwSpawn;
        StartCoroutine(IncreaseSpeed());
    }

    private void Update()
    {
        if (_timeBtwSpawn <= 0)
        {
            SpawnObject();
            _timeBtwSpawn = _startTimeBtwSpawn;
        }
        else
        {
            _timeBtwSpawn -= Time.deltaTime;
        }
    }

    private void SpawnObject()
    {
        // Get the bounds of the BoxCollider2D
        Bounds bounds = _spawnArea.bounds;

        // Pick a random X within the collider's width
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        
        // Use the collider's Y position as the spawn height
        Vector3 spawnPos = new(randomX, transform.position.y, 0);

        GameObject balloon = Instantiate(_obstacle[Random.Range(0, _obstacle.Length)], spawnPos, Quaternion.identity);
        
        if (balloon.TryGetComponent(out Balloon balloonScript))
        {
            balloonScript.SetBalloonSpeed(_balloonSpeed);
        }
    }

    private void FitColliderToScreenWidth()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        // Calculate screen width in world units at the spawner's distance
        float screenHeight = 2f * cam.orthographicSize;
        float screenWidth = screenHeight * cam.aspect;

        // Set the BoxCollider size to match the screen width
        _spawnArea.size = new Vector2(screenWidth, 1f); 
    }

    private IEnumerator IncreaseSpeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            
            if (_balloonSpeed < 100)
            {
                _balloonSpeed += 5;
            }
            
            if (_startTimeBtwSpawn > _minTime)
            {
                _startTimeBtwSpawn -= _decreaseTime;
            }
        }
    }
}