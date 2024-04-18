using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject _balloonPrefab;
    void Start()
    {
        Instantiate(_balloonPrefab, transform.position, Quaternion.identity);
    }

 
}
