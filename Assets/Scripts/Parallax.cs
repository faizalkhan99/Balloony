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
        transform.position += _speed * Time.deltaTime * new Vector3(-1, 0, 0);
        if(transform.position.x >= _xEndCoordinate)
        {
            transform.position = new(_xStartCoordinate, transform.position.y, transform.position.z);
        }
    }
}
