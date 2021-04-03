using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris3D.View
{
    public class CameraView : TetrisElement
    {
        #region Data

        #endregion

        #region Unity Methods
        private void Update()
        {
            CheckCameraInput();
        }

        /// <summary>
        /// Check for input to rotate camera
        /// </summary>
        private void CheckCameraInput()
        {
            if (Input.GetMouseButton(1))
            {
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");

                app.Notify(TetrisNotifications.RotateCameraInput, this, mouseX, mouseY);
            }
        }
        #endregion
    }
}