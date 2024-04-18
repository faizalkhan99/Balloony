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

    void Update()
    {
        if(_timeBtwSpawn <=0)
        {
            Vector3 randPos = new Vector3(Random.Range(_SpawnRangeMinX, _SpawnRangeMaxX), -26,0);
            Instantiate(_objstacle[Random.Range(0, _objstacle.Length)], randPos, Quaternion.identity);
            _timeBtwSpawn = _startTimeBtwSpawn;
            if (_startTimeBtwSpawn > _minTime)
            {
                _timeBtwSpawn -= _decreaseTime;
            }
        }
        else
        {
            _timeBtwSpawn -= Time.deltaTime;
        }
    }
}
