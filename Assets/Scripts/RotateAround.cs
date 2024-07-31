using UnityEngine;

public class RotateAround : MonoBehaviour
{
    private int _rotationSpeed;

    private void Start()
    {
        _rotationSpeed = Random.Range(80, 100);
    }

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.up * _rotationSpeed * Time.fixedDeltaTime);
    }
}
