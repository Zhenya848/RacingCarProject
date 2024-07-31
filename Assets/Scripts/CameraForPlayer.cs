using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraForPlayer : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private CarControllerOld _carController;

    [SerializeField] private float _fieldOfViewKoef;

    private void Start()
    {
        _camera = GetComponent<Camera>(); 
    }

    private void Update()
    {
        _camera.fieldOfView = Mathf.Sqrt(_carController.Speed) * _fieldOfViewKoef + 60;
    }
}