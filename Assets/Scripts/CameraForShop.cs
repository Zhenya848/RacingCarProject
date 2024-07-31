using UnityEngine;
using System.Collections;

public class CameraForShop : MonoBehaviour
{
    private Transform _focalPoint;

    private Vector2 _tapPosition;
    private Vector2 _startCameraPos;
    private Vector2 _currentCameraPos;
    private Vector2 _swipeDelta;

    [SerializeField] private float _sensivityOfScroll;
    [SerializeField] private float _sensivityOfCamera;
    private float _currentVelosityX;

    private void Start()
    {
        _focalPoint = GetComponent<Transform>();
        _currentCameraPos = new Vector2(transform.localEulerAngles.y, 0);
    }

    private void Update()
    {
        if (Input.touchCount < 2)
            MouseMove();
    }

    public void TurnTheCamera(float angle)
    {
        IEnumerator TurnDelay()
        {
            _currentCameraPos.x = _currentCameraPos.x % 360;

            for (int i = 0; i < Mathf.Abs(angle - _currentCameraPos.x) + 1; i++)
            {
                _currentCameraPos.x = Mathf.SmoothDamp(_currentCameraPos.x, angle, ref _currentVelosityX, 0.1f);
                _focalPoint.rotation = Quaternion.Euler(0, _currentCameraPos.x, 0);

                yield return null;
            }
        }

        StartCoroutine(TurnDelay());
    }

    private void MouseMove()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _tapPosition = Input.mousePosition;
            _startCameraPos = _currentCameraPos;
        }

        if (Input.GetMouseButton(0))
        {
            _swipeDelta = ((Vector2)Input.mousePosition - _tapPosition) * _sensivityOfCamera;
            _currentCameraPos.x = Mathf.SmoothDamp(_currentCameraPos.x, _startCameraPos.x + _swipeDelta.x, ref _currentVelosityX, 0.1f);

            _focalPoint.rotation = Quaternion.Euler(0, _currentCameraPos.x, 0);
        }
    }
}