using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tetris3D.Models;
using Tetris3D.View;
using Tetris3D.Controllers;

namespace Tetris3D
{
    public class TetrisApplication : MonoBehaviour
    {
        #region Data
        public TetrisModel model;
        public TetrisView view;
        public TetrisController controller;
        public CameraController camController;
        #endregion

        #region Unity Methods

        #endregion

        #region Custom Methods
        public void Notify(string p_event_path, Object p_target, params object[] p_data)
        {
            foreach (Controller ctrl in GetAllControllers())
            {
                ctrl.OnNotification(p_event_path, p_target, p_data);
            }
        }

        private Controller[] GetAllControllers()
        {
            return GameObject.FindObjectsOfType<Controller>();
        }
        #endregion
    }
}