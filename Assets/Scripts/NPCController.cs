using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCController : MonoBehaviour
{
    private Transform[] _componentsOfCar;
    [SerializeField] private Transform _kuzov;
    [SerializeField] private Transform _motors;
    [SerializeField] private Transform _ruls;
    [SerializeField] private Transform _spoilers;
    [SerializeField] private Transform _glushitels;
    [SerializeField] private Transform _wheels;

    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _zakis;
    [SerializeField] private GameObject _axis;

    public int NpsLocalSpeed = 0;

    [SerializeField] private Transform[] _frontWheels;
    [SerializeField] private Transform[] _rearWheels;
    private Transform[] _wheelMeshes;

    public bool IsActive = true;

    private void Start()
    {
        NpsLocalSpeed = Random.Range(10, 12);
        _componentsOfCar = new Transform[] { _kuzov, _motors, _ruls, _spoilers, _glushitels, _wheels };

        for (int i = 0; i < _componentsOfCar.Length; i++)
            GetRandomDetail(i);

        _frontWheels = GetWheels(_wheels, true);
        _rearWheels = GetWheels(_wheels, false);

        _wheelMeshes = new Transform[_frontWheels.Length + _rearWheels.Length];

        _frontWheels.CopyTo(_wheelMeshes, 0);
        _rearWheels.CopyTo(_wheelMeshes, _frontWheels.Length);
    }

    private void GetRandomDetail(int indexOfDetail)
    {
        Transform currentComponent = _componentsOfCar[indexOfDetail];
        List<GameObject> childsOfComponent = new List<GameObject>();

        int randomItemIndex = Random.Range(0, currentComponent.childCount);
        int destroyItemsCount = 0;

        for (int i = 0; i < currentComponent.childCount; i++)
            childsOfComponent.Add(currentComponent.GetChild(i).gameObject);

        for (int i = 0; i < childsOfComponent.Count; i++)
        {
            if (i + destroyItemsCount != randomItemIndex)
            {
                DestroyImmediate(childsOfComponent[i]);
                childsOfComponent.RemoveAt(i);
                i--;
                destroyItemsCount++;
            }
        }
    }

    private Transform[] GetWheels(Transform wheelsParent, bool frontWheels)
    {
        Transform wheels = wheelsParent.GetChild(0);

        return frontWheels ? new Transform[] { wheels.GetChild(0), wheels.GetChild(1) } : new Transform[] { wheels.GetChild(2), wheels.GetChild(3) };
    }

    private void FixedUpdate()
    {
        if (IsActive == false)
            return;

        transform.Translate(Vector3.forward * NpsLocalSpeed * Time.fixedDeltaTime);
        WheelsRotate();
    }

    private void WheelsRotate()
    {
        foreach (Transform wheel in _wheelMeshes)
            wheel.transform.Rotate(Vector3.right * NpsLocalSpeed * Time.fixedDeltaTime * 100);
    }

    public void DeadAnim(Vector3 playerDirection, float playerSpeed)
    {
        IsActive = false;
        gameObject.tag = "Untagged";

        StartCoroutine(DestroyNpc(3));
        Destroy(_zakis);
        Destroy(_axis);
        Destroy(_player);
        Destroy(_motors.GetChild(0).gameObject);

        for (int i = 0; i < _componentsOfCar.Length; i++)
        {
            Transform component = _componentsOfCar[i];
            GameObject child;

            if (i == 4 || i == 5)
            {
                component = component.GetChild(0);

                for (int j = 0; j < component.childCount; j++)
                {
                    child = component.GetChild(j).GetChild(0).gameObject;
                    FlyingApart(child, playerDirection, playerSpeed);
                }

                continue;
            }

            child = component.GetChild(0).gameObject;
            FlyingApart(child, playerDirection, playerSpeed);
        }
    }

    private void FlyingApart(GameObject child, Vector3 playerPosition, float playerSpeed)
    {
        Rigidbody rb;
        int npcRelativeSpeed = transform.rotation.y == 1 ? NpsLocalSpeed : -NpsLocalSpeed;

        child.AddComponent<Rigidbody>();
        child.AddComponent<MeshCollider>();

        child.GetComponent<MeshCollider>().cookingOptions = MeshColliderCookingOptions.UseFastMidphase;
        child.GetComponent<MeshCollider>().convex = true;
        rb = child.GetComponent<Rigidbody>();

        rb.AddForce((transform.position - playerPosition) * (playerSpeed + npcRelativeSpeed), ForceMode.Impulse);
        rb.AddTorque(RandomTorque(40), RandomTorque(40), RandomTorque(40), ForceMode.Impulse);
    }

    IEnumerator DestroyNpc(int delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private int RandomTorque(int value)
    {
        return Random.Range(-value, value);
    }
}