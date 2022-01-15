using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BackCamera : MonoBehaviour
{
    #region Variables

    public Vector3 offset;
    public float lookAtHeight = 2f;
    public float smoothSpeed = 0.5f;
    private float distance;

    private float x;
    public float xRotSpeed = 10.0f;

    private Vector3 boxPosition;
    public Vector3 boxSize = new Vector3(4, 2, 2);

    public Transform target;

    public LayerMask obstacleMask;

    private Vector3 refVelocity;

    #endregion Variables

    private void Start()
    {
        distance = Vector3.Distance(transform.position, target.position);
        offset = offset.normalized;
        transform.position = target.position + offset * distance;
    }

    private void LateUpdate()
    {
        HandleCamera();

        boxPosition = transform.position;
        boxPosition.z += 2f;

        Collider[] targets = Physics.OverlapBox(boxPosition, boxSize * 0.5f, Quaternion.Euler(transform.forward), obstacleMask);

        foreach (Collider target in targets)
        {
            StartCoroutine(disableRayHitObject(target.gameObject));
        }
    }

    IEnumerator disableRayHitObject(GameObject go)
    {
        MeshRenderer renderer = go.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;

            yield return new WaitForSeconds(1.0f);

            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
        else
            yield return null;
    }

    public void HandleCamera()
    {
        if (!target) return;

        if (Input.GetMouseButton(1))
        {
            x = Input.GetAxis("Mouse X") * xRotSpeed;

            transform.RotateAround(target.transform.position, Vector3.up, x);

            offset = (transform.position - target.position).normalized;
        }

        Vector3 finalPosition = target.position + offset * distance;
        //float curDis = Vector3.Distance(finalPosition, target.position);
        //if (curDis < distance)
        //    finalPosition += (target.position - transform.position).normalized * (curDis - distance);

        Vector3 lookAtPosition = target.position;
        lookAtPosition.y += lookAtHeight;

        transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, smoothSpeed);
        transform.LookAt(lookAtPosition);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        if (target)
        {
            Vector3 lookAtPosition = target.position;
            lookAtPosition.y += lookAtHeight;
            Gizmos.DrawLine(transform.position, lookAtPosition);
            Gizmos.DrawSphere(lookAtPosition, 0.25f);
        }
        Gizmos.DrawSphere(transform.position, 0.25f);
    }
}
