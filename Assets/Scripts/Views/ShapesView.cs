using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tetris3D;

namespace Tetris3D.View
{
    public class ShapesView : TetrisElement
    {
        #region Data

        #endregion

        #region Unity Methods
        private void Update()
        {
            switch (app.model.MatchState)
            {
                case GameState.InProgress:
                    CheckShapesInput(); // You can only move shapes when the game is in progress
                    break;
            }

        }
        #endregion

        #region Custom Methods
        /// <summary>
        /// Receives inputs to move shapes, rotate shapes and speed up their fall.
        /// </summary>
        private void CheckShapesInput()
        {
            #region Movement
            Vector3 axisMovement = Vector3.zero;

            if (Input.GetKeyDown(KeyCode.A))
            {
                axisMovement += Vector3.left;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                axisMovement += Vector3.right;
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                axisMovement += Vector3.forward;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                axisMovement += Vector3.back;
            }

            if (axisMovement != Vector3.zero)
            {
                app.Notify(TetrisNotifications.MoveShape, this, axisMovement);
            }
            #endregion

            #region Rotation
            Vector3 axisRotation = Vector3.zero;

            if (Input.GetKeyDown(KeyCode.Q))
            {
                axisRotation += Vector3.up;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                axisRotation += Vector3.down;
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                axisRotation += Vector3.right;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                axisRotation += Vector3.left;
            }

            if (axisRotation != Vector3.zero)
            {
                app.Notify(TetrisNotifications.RotateShape, this, axisRotation);
            }
            #endregion

            #region Speed Up Fall
            if (Input.GetKeyDown(KeyCode.Space))
            {
                app.Notify(TetrisNotifications.SpeedUpFall, this, true);
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                app.Notify(TetrisNotifications.SpeedUpFall, this, false);
            }
            #endregion
        }
        #endregion
    }
}