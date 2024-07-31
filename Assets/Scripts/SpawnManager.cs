using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Transform _road;
    [SerializeField] private Transform _player;

    [SerializeField] private GameObject _money;
    public bool IsSpawnActive;

    private void Start()
    {
        StartCoroutine(SpawnDelay());
    }

    private IEnumerator SpawnDelay()
    {
        while (IsSpawnActive)
        {
            yield return new WaitForSeconds(Random.Range(5, 8));

            Vector3 pos = new Vector3(_player.position.x, 1, 120);
            Instantiate(_money, pos, _money.transform.rotation).transform.SetParent(_road);
        }
    }
}