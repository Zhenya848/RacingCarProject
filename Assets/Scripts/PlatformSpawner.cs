using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private Transform _road;
    [SerializeField] private CarControllerOld _carController;

    private List<GameObject[]> _bioms;
    private GameObject[] _currentBiom;
    [SerializeField] private GameObject[] _simplePlatforms;
    [SerializeField] private GameObject[] _desertPlatforms;
    private int _indexOfBiom;

    private int _biomChangeTime = 12;
    private float _biomDuration = 12;

    [SerializeField] private List<GameObject> _platformsList;
    public Transform LastPlatform;

    [SerializeField] private float _deletePointZ;

    private void Start()
    {
        _bioms = new List<GameObject[]>() { _simplePlatforms, _desertPlatforms };
        _indexOfBiom = Random.Range(0, _bioms.Count);

        _currentBiom = _bioms[_indexOfBiom];

        AddPlatform();
        AddPlatform();
        AddPlatform();
        AddPlatform();
        AddPlatform();

        _deletePointZ = GetDeletePointZ();
    }

    private void FixedUpdate()
    {
        _biomDuration += Time.fixedDeltaTime;
        _road.transform.Translate(Vector3.back * Time.fixedDeltaTime * _carController.Speed);

        if (_platformsList[0].transform.position.z < _deletePointZ)
        {
            Destroy(_platformsList[0]);
            _platformsList.RemoveAt(0);

            AddPlatform();
            _deletePointZ = GetDeletePointZ();
        }
    }

    private void AddPlatform()
    {
        int indexOfPlatform = GetIndexOfPlatform();
        Vector3 spawnPoint = GetSpawnPoint();

        GameObject newPlatform = Instantiate(GetPlatform(_currentBiom[indexOfPlatform]), spawnPoint, transform.rotation);
        LastPlatform = newPlatform.GetComponent<Transform>();

        _platformsList.Add(newPlatform);
        newPlatform.transform.SetParent(_road);
    }

    private int GetIndexOfPlatform()
    {
        int indexOfPlatform = Random.Range(0, _currentBiom.Length);

        if (_currentBiom[indexOfPlatform].name == "RoadPerekrestok" || _currentBiom[indexOfPlatform].name == "IronRoad_01a")
        {
            foreach (GameObject platform in _platformsList)
            {
                if (platform.name == "RoadPerekrestok(Clone)" || platform.name == "IronRoad_01a(Clone)")
                    return 0;
            }
        }

        return indexOfPlatform;
    }

    private GameObject GetPlatform(GameObject defaultPlatform)
    {
        if (_biomDuration >= _biomChangeTime)
        {
            _biomDuration = 0;
            _biomChangeTime = Random.Range(10, 32);

            _indexOfBiom = _indexOfBiom == _bioms.Count - 1 ? 0 : _indexOfBiom + 1;
            _currentBiom = _bioms[_indexOfBiom];

            return _currentBiom[0];
        }

        return defaultPlatform;
    }

    private Vector3 GetSpawnPoint()
    {
        if (LastPlatform)
        {
            for (int i = 0; i < LastPlatform.childCount; i++)
            {
                if (LastPlatform.GetChild(i).name == "SpawnPoint")
                    return LastPlatform.GetChild(i).position;
            }
        }

        return new Vector3(0, 0, -4);
    }

    private float GetDeletePointZ()
    {
        Transform firstPlatform = _platformsList[0].transform;

        if (firstPlatform)
        {
            for (int i = 0; i < firstPlatform.transform.childCount; i++)
            {
                if (firstPlatform.GetChild(i).name == "SpawnPoint")
                    return -firstPlatform.GetChild(i).localPosition.z - 8;
            }
        }

        return _deletePointZ;
    }
}
