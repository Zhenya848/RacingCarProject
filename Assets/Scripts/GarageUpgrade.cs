using System.Collections.Generic;
using UnityEngine;

public class GarageUpgrade : MonoBehaviour
{
    [SerializeField] private Shelve[] _shelves;
    [SerializeField] private GameObject _box;

    private void Start()
    {
        foreach (Shelve shelve in _shelves)
            shelve.Initialize(_box);
    }

    public void AddBox()
    {
        foreach (Shelve shelve in _shelves)
        {
            if (shelve.CanAddBox())
            {
                shelve.AddBox();
                break;
            }
        }
    }
}

[System.Serializable]
class Shelve
{
    [SerializeField] private List<Transform> _pointsToSpawn;
    [SerializeField] private string _tag;
    private GameObject _box;
    private int _spawnedCount;

    public void Initialize(GameObject box)
    {
        _box = box;
        _spawnedCount = PlayerPrefs.HasKey(_tag) ? PlayerPrefs.GetInt(_tag) : 0;

        for (int i = 0; i < _spawnedCount; i++)
            GameObject.Instantiate(_box, _pointsToSpawn[i].position, _box.transform.rotation);

        _pointsToSpawn.RemoveRange(0, _spawnedCount);
    }

    public void AddBox()
    {
        if (_pointsToSpawn.Count > 0)
        {
            GameObject.Instantiate(_box, _pointsToSpawn[0].position, _box.transform.rotation);
            _pointsToSpawn.RemoveAt(0);

            PlayerPrefs.SetInt(_tag, ++_spawnedCount);
        }
    }

    public bool CanAddBox()
    {
        return _pointsToSpawn.Count > 0;
    }
}
