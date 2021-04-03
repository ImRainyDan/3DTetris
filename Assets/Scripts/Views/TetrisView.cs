using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris3D.View
{
    public class TetrisView : TetrisElement
    {
        #region Data
        public GameObject GridContainer; // The visual representation of the main grid
        public ShapesView Shapes; // The object containing all the visual shapes and blocks
        #endregion

        #region Unity Methods
        private void Update()
        {
            switch (app.model.MatchState)
            {
                case GameState.EndingViewScore:
                    CheckGameEndInput(); 
                    break;
            }
        }
        #endregion

        #region Custom Methods
        /// <summary>
        /// Check for input to restart the game when it's over.
        /// </summary>
        private void CheckGameEndInput()
        {
            if (Input.anyKeyDown)
            {
                if (!Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2))
                {
                    app.Notify(TetrisNotifications.RestartGame, this);
                }
            }
        }
        #endregion
    }
}