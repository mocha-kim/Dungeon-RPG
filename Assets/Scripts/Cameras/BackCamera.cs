using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BackCamera : MonoBehaviour
{
    #region Variables

    public float height = 1f;
    public float distance = 3f;
    public float lookAtHeight = 2f;
    public float smoothSpeed = 0.5f;

    private float x;
    public float xRotSpeed = 160.0f;

    private Vector3 boxPosition;
    public Vector3 boxSize = new Vector3(4, 2, 2);
    Quaternion rotation;

    public Transform target;

    public LayerMask obstacleMask;

    private Vector3 refVelocity;

    #endregion Variables

    private void Start()
    {
        Vector3 angles = transform.eulerAngles;

        x = angles.y;

        rotation = Quaternion.AngleAxis(-180f, Vector3.up);

        HandleCamera();
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
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;

        yield return new WaitForSeconds(1.0f);

        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    }

    public void HandleCamera()
    {
        if (!target) return;

        if (Input.GetMouseButton(1))
        {
            x += Input.GetAxis("Mouse X") * xRotSpeed * distance * 0.02f;

            rotation = Quaternion.Euler(0, x, 0);
            transform.rotation = rotation;
        }
        // Calc world position vector
        Vector3 worldPosition = (Vector3.forward * distance) + (Vector3.up * height);
        //Debug.DrawLine(target.position, worldPosition, Color.red);

        // Calc rotate vector
        Vector3 rotatedVector = rotation * worldPosition;
        //Debug.DrawLine(target.position, rotatedVector, Color.green);

        // Move camera position
        Vector3 flatTargetPosition = target.position;
        flatTargetPosition.y += lookAtHeight;

        Vector3 finalPosition = flatTargetPosition + rotatedVector;
        //Debug.DrawLine(target.position, finalPosition, Color.blue);

        transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, smoothSpeed);
        transform.LookAt(flatTargetPosition);
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
