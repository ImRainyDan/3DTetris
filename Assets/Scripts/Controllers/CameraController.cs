using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tetris3D.View;
using Tetris3D.Models;

namespace Tetris3D.Controllers
{
    public class CameraController : Controller
    {
        #region Data
        public CameraModel model;
        public CameraView view;
        #endregion

        #region Unity Methods
        private void Start()
        {
            Vector3 lookAtPos = GetLookAtPosition();

            // Initialize camera position
            Vector3 dir = new Vector3(0, 0, -model.Distance);
            Quaternion rotation = Quaternion.Euler(model.CurrentY, model.CurrentX, 0);
            view.transform.position = lookAtPos + rotation * dir;
            view.transform.LookAt(lookAtPos);
        }

        private void LateUpdate()
        {
            if (model.IsEnabled)
            {
                if (model.Target != null && view != null)
                {
                    UpdateCameraPos();
                }
            }
        }
        #endregion

        #region Custom Methods
        public override void OnNotification(string eventPath, Object eventSource, params object[] eventData)
        {
            switch (eventPath)
            {
                case TetrisNotifications.RotateCameraInput:
                    float mouseX = (float)eventData[0];
                    float mouseY = (float)eventData[1];

                    OnPlayerRotateCamera(mouseX, mouseY);
                    break;
            }
        }

        /// <summary>
        /// Calculates the angle in which the camera is facing the grid, rounded to 90 degrees.
        /// </summary>
        /// <returns>Angle in which camera is facing the grid, rounded to 90 degrees.</returns>
        public float GetCameraAngleFactor()
        {
            return (int)((model.CurrentX + (45.0f * Mathf.Sign(model.CurrentX))) / 90.0f) * 90.0f;
        }

        /// <summary>
        /// Player input event, when the camera is being rotated
        /// </summary>
        /// <param name="mouseX">Distance moved in the X grid direction</param>
        /// <param name="mouseY">Distance moved in the Y grid direction</param>
        private void OnPlayerRotateCamera(float mouseX, float mouseY)
        {
            if (model.IsEnabled)
            {
                if (model.Target != null && view != null)
                {
                    model.CurrentX += mouseX * model.HorizontalSpeed;
                    model.CurrentY += mouseY * model.VerticalSpeed;

                    model.CurrentY = Mathf.Clamp(model.CurrentY, CameraModel.YAngleMin, CameraModel.YAngleMax);
                }
            }
        }

        /// <summary>
        /// Calculates and updates the new camera position, facing the "Look At" target.
        /// </summary>
        private void UpdateCameraPos()
        {
            Vector3 lookAtPos = GetLookAtPosition();
            Vector3 dir = new Vector3(0, 0, -model.Distance);
            Quaternion rotation = Quaternion.Euler(model.CurrentY, model.CurrentX, 0);

            view.transform.position = lookAtPos + rotation * dir;
            view.transform.LookAt(lookAtPos);
        }

        /// <summary>
        /// Calculates the position the camera needs to look at.
        /// </summary>
        /// <returns>The Vector3 position the camera needs to look at.</returns>
        private Vector3 GetLookAtPosition()
        {
            return model.Target.position + model.CameraOffset;
        }
        #endregion
    }
}