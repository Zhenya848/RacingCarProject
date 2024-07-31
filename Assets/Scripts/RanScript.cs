using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RanScript : MonoBehaviour
{
    [SerializeField] private GameObject _flySwatters;

    private void OnEnable()
    {
        _flySwatters.SetActive(false);
    }

    private void OnDisable()
    {
        _flySwatters.SetActive(true);
    }
}
