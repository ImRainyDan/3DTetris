using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris3D.View
{
    public class UIView : TetrisElement
    {
        #region Data
        public Text LevelText; // HUD Level Display
        public Text ScoreText; // HUD Score Display 
        public Transform PreviewContainer; // HUD Container for the next shape preview
        public GameObject EndingContainer; // Object containing all ending-related UI elements
        public GameObject InputContainer; // Container for the "Save Score" screen
        public GameObject ScoreContainer; // Container for the "View Scores" screen
        public GameObject ScoresListContainer; // Container for the list of highscores
        public InputField PlayerNameField; // The player name input field in the "Save Score" screen
        #endregion

        #region Unity Methods
        #endregion

        #region Custom Methods
        /// <summary>
        /// Player input event for the "Save Score" button
        /// </summary>
        public void OnSaveButtonClick()
        {
            app.Notify(TetrisNotifications.SaveHighscore, this, PlayerNameField.text);
        }

        /// <summary>
        /// Player input event for the "Don't Save Score" button
        /// </summary>
        public void OnDontSaveButtonClick()
        {
            app.Notify(TetrisNotifications.DontSaveHighscore, this);
        }
        #endregion
    }
}