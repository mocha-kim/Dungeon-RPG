using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity_RPG.Cameras
{

    [ExecuteInEditMode]
    public class TopDownCamera : MonoBehaviour
    {
        #region Variables

        public float height = 5f;
        public float distance = 10f;
        public float angle = 45f;
        public float lookAtHeight = 2f;
        public float smoothSpeed = 0.5f;

        public Transform target;

        private Vector3 refVelocity;

        #endregion Variables

        private void Start()
        {
            HandleCamera();
        }

        private void LateUpdate()
        {
            HandleCamera();
        }

        public void HandleCamera()
        {
            if (!target) return;

            // Calc world position vector
            Vector3 worldPosition = (Vector3.forward * -distance) + (Vector3.up * height);
            Debug.DrawLine(target.position, worldPosition, Color.red);

            // Calc rotate vector
            Vector3 rotatedVector = Quaternion.AngleAxis(angle, Vector3.up) * worldPosition;
            Debug.DrawLine(target.position, rotatedVector, Color.green);

            // Move camera position
            Vector3 flatTargetPosition = target.position;
            flatTargetPosition.y += lookAtHeight;

            Vector3 finalPosition = flatTargetPosition + rotatedVector;
            Debug.DrawLine(target.position, finalPosition, Color.blue);

            transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, smoothSpeed);
            transform.LookAt(target.transform);
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

}