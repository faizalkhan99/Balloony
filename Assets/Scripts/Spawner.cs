using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _objstacle;

    [SerializeField] private float _timeBtwSpawn;
    [SerializeField] private float _startTimeBtwSpawn;
    [SerializeField] private float _decreaseTime;
    [SerializeField] private float _minTime;

    [SerializeField] private float _SpawnRangeMinX;
    [SerializeField] private float _SpawnRangeMaxX;

    [SerializeField] private int _balloonSpeed;

    
    private void Start()
    {
        StartCoroutine(IncreaseSpeed());
    }
    void Update()
    {
        if (_timeBtwSpawn <= 0)
        {
            Vector3 randPos = new(Random.Range(_SpawnRangeMinX, _SpawnRangeMaxX), -26, 0);
            GameObject balloon = Instantiate(_objstacle[Random.Range(0, _objstacle.Length)], randPos, Quaternion.identity);
            if (balloon != null)
            {
                balloon.GetComponent<Balloon>().SetBalloonSpeed(_balloonSpeed);
            }
            _timeBtwSpawn = _startTimeBtwSpawn;
            
        }
        else
        {
            _timeBtwSpawn -= Time.deltaTime;
        }
    }
    private IEnumerator IncreaseSpeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            if (_balloonSpeed <= 100)
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
