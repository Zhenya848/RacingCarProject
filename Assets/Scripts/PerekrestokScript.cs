using UnityEngine;

public class PerekrestokScript : MonoBehaviour
{
    private Transform _player;
    [SerializeField] private GameObject _npc;

    [SerializeField] private Transform _toRightLane0;
    [SerializeField] private Transform _toRightLane1;
    [SerializeField] private Transform _toLeftLane0;
    [SerializeField] private Transform _toLeftLane1;

    private void Start()
    {
        _player = GameObject.Find("Player").transform;
    }

    private void FixedUpdate()
    {
        if (_player.position.z > transform.position.z - _player.GetComponent<CarControllerOld>().Speed * 1.44f)
        {
            int randomPoint = Random.Range(0, 2);

            GameObject npc0 = Instantiate(_npc, randomPoint == 0 ? _toLeftLane0.position : _toLeftLane1.position, Quaternion.Euler(0, -90, 0));
            GameObject npc1 = Instantiate(_npc, randomPoint == 0 ? _toRightLane1.position : _toRightLane0.position, Quaternion.Euler(0, 90, 0));

            npc0.transform.SetParent(transform);
            npc1.transform.SetParent(transform);

            Destroy(this);
        }
    }
}