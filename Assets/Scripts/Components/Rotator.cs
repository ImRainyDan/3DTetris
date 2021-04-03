using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris3D.Components
{
    public class Rotator : MonoBehaviour
    {
        #region Data
        public float RotationSpeed = 60; // How many angles per second does this object rotate
        public Vector3 Axis = Vector3.up; // The rotation axis
        #endregion

        #region Unity Methods
        private void Update()
        {
            transform.Rotate(Axis, RotationSpeed * Time.deltaTime, Space.Self);
        }
        #endregion
    }
}