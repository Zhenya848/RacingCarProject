using UnityEngine;
using System.Collections.Generic;

public class FeaturesOfCar : MonoBehaviour
{
    private CarControllerOld _carController;
    private InteractionManager _interactionManager;

    private int _maxSpeed;
    private int _turningSpeed;
    private int _health;

    [SerializeField] private Transform _kuzov;
    [SerializeField] private Transform _motors;
    [SerializeField] private Transform _ruls;
    [SerializeField] private Transform _spoilers;
    [SerializeField] private Transform _glushitels;
    [SerializeField] private Transform _wheels;
    [SerializeField] private Transform _flySwatters;
    [SerializeField] private Transform _ran;

    [SerializeField] private GameObject _pilot;
    [SerializeField] private GameObject _zakis;
    [SerializeField] private GameObject _axis;

    public int[] rulCharacteristics = new int[] { 3, 3, 4, 5, 8, 8, 9 };
    public int[] wheelCharacteristics = new int[] { 1, 1, 1, 2, 5, 7, 7, 7 };
    public int[] kuzovCharacteristics = new int[] { 3, 3, 4, 4, 4, 5, 6, 6, 6, 7, 7, 7, 8 };
    public int[] motorCharacteristics = new int[] { 13, 18, 21 };
    public int[] spoilerCharacteristics = new int[] { 0, 5, 5, 5, 8, 12 };

    private Transform[] _componentsOfCar;
    private string[] _tags;

    private void Awake()
    {   
        _carController = GetComponent<CarControllerOld>();
        _interactionManager = GetComponent<InteractionManager>();

        _componentsOfCar = new Transform[] { _kuzov, _motors, _ruls, _spoilers, _glushitels, _wheels, _flySwatters, _ran };
        _tags = new string[] { "Kuzov", "Motor", "Rul", "Spoiler", "Glushitel", "Wheels", "FlySwatter", "Ran" };

        _maxSpeed = GetCharacteristic(motorCharacteristics, _tags[1]) + GetCharacteristic(wheelCharacteristics, _tags[5]) + 
                    GetCharacteristic(spoilerCharacteristics, _tags[3]) + GetCharacteristic(kuzovCharacteristics, _tags[0]);

        _turningSpeed = GetCharacteristic(rulCharacteristics, _tags[2]);
        _health = GetCharacteristic(kuzovCharacteristics, _tags[0]);

        for (int i = 0; i < _componentsOfCar.Length; i++)
            InitializeComponent(i);

        _carController.Initialize(GetWheels(_wheels, true), GetWheels(_wheels, false), _maxSpeed, _turningSpeed);
        _interactionManager.InitializeHealth(_health);
    }

    private int GetCharacteristic(int[] componentCharacteristics, string tagOfComponent)
    {
        int indexOfDetail = GetSaveValue(tagOfComponent);

        for (int i = 0; i <= indexOfDetail; i++)
        {
            try { return componentCharacteristics[indexOfDetail - i]; }
            catch { Debug.LogWarning($"Характеристика компонента: {tagOfComponent} элемента: {indexOfDetail - i} отсутствует!"); }
        }

        return 0;
    }

    private void InitializeComponent(int indexOfComponent)
    {
        Transform currentComponent = _componentsOfCar[indexOfComponent];
        List<GameObject> childsOfComponent = new List<GameObject>();

        int indexOfDetail = GetSaveValue(_tags[indexOfComponent]);
        int countOfDestroyDetails = 0;

        for (int i = 0; i < currentComponent.childCount; i++)
            childsOfComponent.Add(currentComponent.GetChild(i).gameObject);

        for (int i = 0; i < childsOfComponent.Count; i++)
        {
            if (i + countOfDestroyDetails != indexOfDetail)
            {
                DestroyImmediate(childsOfComponent[i]);
                childsOfComponent.RemoveAt(i);
                i--;
                countOfDestroyDetails++;
            }
        }
    }

    private Transform[] GetWheels(Transform wheelsParent, bool frontWheels)
    {
        Transform wheels = wheelsParent.GetChild(0);

        return frontWheels ? new Transform[] { wheels.GetChild(0), wheels.GetChild(1) } : new Transform[] { wheels.GetChild(2), wheels.GetChild(3) };
    }

    private int GetSaveValue(string tag)
    {
        if (PlayerPrefs.HasKey(tag) == false)
            return 0;

        return PlayerPrefs.GetInt(tag);
    }

    public void PlayerDeadAnim(Vector3 objectPosition)
    {
        Destroy(_zakis);
        Destroy(_axis);
        Destroy(_pilot);
        Destroy(_motors.GetChild(0).gameObject);

        for (int i = 0; i < 7; i++)
        {
            Transform component = _componentsOfCar[i];
            GameObject child;

            if (i == 4 || i == 5)
            {
                component = component.GetChild(0);

                for (int j = 0; j < component.childCount; j++)
                {
                    child = component.GetChild(j).GetChild(0).gameObject;
                    FlyingApart(child, objectPosition);
                }

                continue;
            }

            child = component.GetChild(0).gameObject;
            FlyingApart(child, objectPosition);
        }
    }

    private void FlyingApart(GameObject child, Vector3 objectPosition)
    {
        Rigidbody rb;

        child.AddComponent<Rigidbody>();
        child.AddComponent<MeshCollider>();

        child.GetComponent<MeshCollider>().cookingOptions = MeshColliderCookingOptions.UseFastMidphase;
        child.GetComponent<MeshCollider>().convex = true;
        rb = child.GetComponent<Rigidbody>();

        rb.AddForce((transform.position - objectPosition) * 12, ForceMode.Impulse);
        rb.AddTorque(RandomTorque(40), RandomTorque(40), RandomTorque(40), ForceMode.Impulse);
    }

    private int RandomTorque(int value)
    {
        return Random.Range(-value, value);
    }
}