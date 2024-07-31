using System.Collections;
using UnityEngine;

public class Traffic : MonoBehaviour
{
    [SerializeField] private Transform _road;
    [SerializeField] private GameObject _npc;

    [SerializeField] private Transform _player;
    [SerializeField] private GameObject _oncommingCar;
    [SerializeField] private GameObject _passingCar;
    [SerializeField] private GameObject _passingCar2;

    public bool IsTrafficActive = true;
    private int _spawnDelay;

    private void Start()
    {
        StartCoroutine(SpawnNpc());
    }

    private IEnumerator SpawnNpc()
    {
        while (IsTrafficActive)
        {
            _spawnDelay = Random.Range(0, 5);

            if (_oncommingCar == false)
            {
                _oncommingCar = Instantiate(_npc, new Vector3(Random.Range(-3.4f, -5f), 0, 120), new Quaternion(0, 180, 0, 0));
                _oncommingCar.transform.SetParent(_road);
            }

            if (_passingCar == false && _player.GetComponent<CarControllerOld>().Speed > 11)
            {
                _passingCar = Instantiate(_npc, new Vector3(Random.Range(3.4f, 5f), 0, 100), Quaternion.identity);
                _passingCar.transform.SetParent(_road);
            }

            if (_passingCar2 == false && _player.position.x <= 0)
            {
                _passingCar2 = Instantiate(_npc, new Vector3(Random.Range(3.4f, 5f), 0, -10), Quaternion.identity);
                _passingCar2.transform.SetParent(_road);
            }

            yield return new WaitForSeconds(_spawnDelay);
        }
    }
}
