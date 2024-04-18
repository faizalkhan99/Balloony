using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private GameObject _movingObject;

    [SerializeField] private float _speed;

    [SerializeField] private float _xEndCoordinate;
    [SerializeField] private float _xStartCoordinate;
    void Update()
    {
        transform.position += new Vector3(-1, 0, 0) * _speed * Time.deltaTime;
        if(transform.position.x >= _xEndCoordinate)
        {
            transform.position = new Vector3(_xStartCoordinate, transform.position.y, transform.position.z);
        }
    }
}
