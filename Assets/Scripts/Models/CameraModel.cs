using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tetris3D.View;

namespace Tetris3D.Models
{
    public class CameraModel : TetrisElement
    {
        #region Data
        // Constants
        public const float YAngleMin = -89.9f;
        public const float YAngleMax = 89.9f;

        public Transform Target; // The target object to for the camera to look at
        public Vector3 CameraOffset = Vector3.zero; // The offset that's added to the Target's position
        public float Distance = 10.0f; // The camera's distance from the target
        public float HorizontalSpeed = 5.0f; // Camera horizontal rotation speed
        public float VerticalSpeed = 5.0f; // Camera vertical rotation speed
        public float CurrentX = 0.0f; // Camera current X position relative to look at position
        public float CurrentY = 0.0f; // Camera current Y position relative to look at position
        public bool IsEnabled = true; // Is the CameraController enabled?
        #endregion

        #region Unity Methods

        #endregion

        #region Custom Methods
        #endregion
    }
}