using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Cameras
{

    public class CameraFacing : MonoBehaviour
    {
        #region Variables

        public Camera referenceCamera;
        public bool reverseFace = false;

        public enum Axis
        {
            up, down, left, right, foward, back
        };

        public Axis axis = Axis.up;

        public Vector3 GetAxis(Axis refAxis)
        {
            switch(refAxis)
            {
                case Axis.down:
                    return Vector3.down;

                case Axis.left:
                    return Vector3.left;

                case Axis.right:
                    return Vector3.right;

                case Axis.foward:
                    return Vector3.forward;

                case Axis.back:
                    return Vector3.back;
            }
            return Vector3.up;
        }

        #endregion Variables

        private void LateUpdate()
        {
            Vector3 targetPosition = transform.position + referenceCamera.transform.rotation * (reverseFace ? Vector3.forward : Vector3.back);
            Vector3 targetOrientation = referenceCamera.transform.rotation * GetAxis(axis);

            transform.LookAt(targetPosition, targetOrientation);
        }
    }

}