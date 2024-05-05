using UnityEngine;

public class IdleRotator : MonoBehaviour
{
    [SerializeField] private int _rotationSpeed;
    void Update()
    {
        transform.Rotate(new Vector3(0,0,_rotationSpeed) * Time.deltaTime);       
    }
}
