using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class CarControllerOld : MonoBehaviour
{
    public GameObject UIComponents;
    [SerializeField] private GameObject _fuelSign;
    private bool _isFuelSign = true;

    [SerializeField] private TextMeshProUGUI _speedText;
    [SerializeField] private TextMeshProUGUI _fuelText;
    [SerializeField] private TextMeshProUGUI _nitroText;

    private int _NORMAL_SPEED = 4;
    private int _currentMaxSpeed;
    private int _maxSpeed;

    private int _motorForce;
    private const int _BRAKE_FORCE = 4;
    private const int _MIN_BRAKE_FORCE = 1;
    private int _currentBrakeForce;
    private int _turningSpeed;
    private int _axleOfFrontWheelsY = 25;

    public float Speed { get; private set; } = 4;
    private float _maxPosX = 6.6f;
    private float _currentVelocity;
    [SerializeField] private float _horizontalInput;
    private float angleOfWheelsX = 0;
    private const float _SENSIVITY_OF_ROTATION = 6.4f;

    [SerializeField] private Transform[] _frontWheels;
    [SerializeField] private Transform[] _rearWheels;

    [SerializeField] private AnimationCurve _turnCurve;
    [SerializeField] private GameObject _turnButtons;
    private int _turnValue;

    private bool _isMobilePlatform;
    public bool IsFuel { get; private set; } = true;

    [SerializeField] private Scrollbar _fuelScrollbar;
    private float _fuelTank;
    private float _currentFuel = 1;

    [SerializeField] private Scrollbar _nitroScrollbar;
    public float CurrentNitro = 0;
    private bool _isBoost = false;
    private bool _wasBoosted = false;

    private bool _accelerate;

    private void Start()
    {
        _currentMaxSpeed = _NORMAL_SPEED;
        _currentBrakeForce = _MIN_BRAKE_FORCE;

        _isMobilePlatform = Application.isMobilePlatform;
        _turnButtons.SetActive(_isMobilePlatform);

        if (_isMobilePlatform)
            StartCoroutine(TurnAngleDelay());

        _fuelTank = PlayerPrefs.HasKey("FuelValue") ? PlayerPrefs.GetFloat("FuelValue") : 0.7f;
    }

    public void Initialize(Transform[] frontWheels, Transform[] rearWheels, int maxSpeed, int turningSpeed)
    {
        _frontWheels = frontWheels;
        _rearWheels = rearWheels;
        _turningSpeed = turningSpeed;
        _maxSpeed = maxSpeed;

        _motorForce = _maxSpeed / 5;
    }

    private void FixedUpdate()
    {
        WheelsAnim();

        Move();
        Brake();
        Boost();

        if (_isMobilePlatform == false)
            _horizontalInput = Input.GetAxis("Horizontal");

        CarRotate();

        _speedText.text = $"Скорость: {Mathf.Round(Speed * 3.6f)} км/ч";

        if (transform.position.x >= _maxPosX)
            transform.position = new Vector3(_maxPosX, transform.position.y, transform.position.z);
        else if (transform.position.x <= -_maxPosX)
            transform.position = new Vector3(-_maxPosX, transform.position.y, transform.position.z);

        //_currentFuel -= (_accelerate ? 0.01f : 0.05f) * Time.deltaTime / (_fuelTank * _fuelTank);

        _fuelText.text = Mathf.Round(_currentFuel * 100).ToString() + '%';
        _fuelScrollbar.size = _currentFuel;

        if (_currentFuel <= 0 && IsFuel)
        {
            IEnumerator StopDelay()
            {
                int deltaSpeed = (int)(Speed / 3);

                UIComponents.SetActive(false);
                _fuelSign.SetActive(false);
                IsFuel = false;

                _NORMAL_SPEED = 1;
                _currentMaxSpeed = _NORMAL_SPEED;
                _currentBrakeForce = _MIN_BRAKE_FORCE * deltaSpeed;

                yield return new WaitForSeconds(4);
                GetComponent<InteractionManager>().DeadMenu();
            }

            StartCoroutine(StopDelay());
        }
        else if (_isFuelSign && _currentFuel <= 0.2f)
        {
            _fuelSign.SetActive(true);
            _isFuelSign = false;
        }
    }

    public void GasPedal(bool accelerate)
    {
        _currentMaxSpeed = accelerate && IsFuel ? _maxSpeed : _NORMAL_SPEED;
        _accelerate = accelerate;
    }

    public void BrakePedal(bool brake)
    {
        _currentBrakeForce = brake && IsFuel ? _BRAKE_FORCE : _MIN_BRAKE_FORCE;
    }

    public void BoostPedal(bool active)
    {
        _isBoost = active && IsFuel;

        if (active == false)
            GasPedal(false);
    }

    public void TurnPedal(int turnValue)
    {
        _turnValue = turnValue;
    }

    IEnumerator TurnAngleDelay()
    {
        while (true)
        {
            _horizontalInput += (_turnValue - _horizontalInput) * Time.fixedDeltaTime * 5;
            yield return null;
        }
    }

    private void WheelsAnim()
    {
        angleOfWheelsX += Time.fixedDeltaTime * 100 * Speed;

        foreach (Transform frontWheel in _frontWheels)
            frontWheel.rotation = Quaternion.Euler(angleOfWheelsX, _horizontalInput * _axleOfFrontWheelsY, 0);

        foreach (Transform rearWheel in _rearWheels)
            rearWheel.rotation = Quaternion.Euler(angleOfWheelsX, 0, 0);
    }

    private void Move()
    {
        if (Speed >= _currentMaxSpeed)
            return;

        Speed += Time.fixedDeltaTime * _motorForce;
    }

    private void Brake()
    {
        if (Speed <= _NORMAL_SPEED + 0.1f)
            return;

        Speed -= Time.fixedDeltaTime * _currentBrakeForce;
    }

    private void Boost()
    {
        if (_isBoost && CurrentNitro > 0)
        {
            if (_wasBoosted == false)
            {
                _motorForce *= 3;
                _currentMaxSpeed = 56;

                _wasBoosted = true;
                _accelerate = true;
            }

            CurrentNitro -= Time.deltaTime * 2;
        }
        else
        {
            if (_wasBoosted)
            {
                _motorForce = _maxSpeed / 5;
                _currentMaxSpeed = _NORMAL_SPEED;

                _wasBoosted = false;
                _accelerate = false;

                _isBoost = false;

                if (CurrentNitro < 1)
                    GasPedal(true);
            }

            if (CurrentNitro < 10)
                CurrentNitro += Speed / 100 * Time.deltaTime;
        }

        _nitroScrollbar.size = CurrentNitro / 10;
        _nitroText.text = Mathf.RoundToInt(CurrentNitro * 10).ToString() + '%';
    }

    private void CarRotate()
    {
        transform.position += new Vector3(_horizontalInput * (Speed / 43) * _turningSpeed * Time.fixedDeltaTime, 0, 0);
        transform.rotation = Quaternion.Euler(0, Mathf.Clamp(Mathf.SmoothDamp(0, Speed * _horizontalInput * _SENSIVITY_OF_ROTATION, ref _currentVelocity, 0.4f), -8, 8), 0);
    }
}