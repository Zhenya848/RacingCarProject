using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUInctancingEnabler : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<MeshRenderer>().SetPropertyBlock(new MaterialPropertyBlock());
        Destroy(this);
    }
}
