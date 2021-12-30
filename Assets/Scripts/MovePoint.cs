using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePoint : MonoBehaviour
{
    public float surfaceOffset = 0.1f;
    public Transform target = null;

    // Update is called once per frame
    private void Update()
    {
        if (target)
        {
            transform.position = target.position + Vector3.up * surfaceOffset;
        }
    }

    public void SetPosition(RaycastHit hit)
    {
        target = null;
        transform.position = hit.point + hit.normal * surfaceOffset;
    }    
}
