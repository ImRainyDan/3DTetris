using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tetris3D.View;
using Tetris3D.Models;
using Tetris3D.Components;
using System.IO;
using Tetris3D.Utility;

namespace Tetris3D.Controllers
{
    public class UIController : Controller
    {
        #region Data
        public UIView view;
        public UIModel model;
        #endregion

        #region Unity Methods

        #endregion

        #region Custom Methods
        public override void OnNotification(string eventPath, Object eventSource, params object[] eventData)
        {
            switch (eventPath)
            {
                case TetrisNotifications.UIUpdateLevel:
                    int newLevel = (int)eventData[0];

                    OnUpdateLevel(newLevel);
                    break;

                case TetrisNotifications.UIUpdateScore:
                    int newScore = (int)eventData[0];

                    OnUpdateScore(newScore);
                    break;

                case TetrisNotifications.UIUpdatePreview:
                    GameObject newPreview = (GameObject)eventData[0];

                    OnUpdatePreview(newPreview);
                    break;

                case TetrisNotifications.GameOver:
                    OnGameOver();
                    break;

                case TetrisNotifications.SaveHighscore:
                    string playerName = (string)eventData[0];

                    OnSaveScore(playerName);
                    break;

                case TetrisNotifications.DontSaveHighscore:
                    OnDontSaveScore();
                    break;

                case TetrisNotifications.DisplayHighScore:
                    DisplayHighscores();
                    break;
            }
        }

        /// <summary>
        /// Player event when the "Save Score" button has been pressed.
        /// </summary>
        /// <param name="playerName">The name of the player who's score needs to be saved.</param>
        public void OnSaveScore(string playerName)
        {
            // Create a new highscore object
            Highscore newScore = new Highscore();
            newScore.Name = playerName;
            newScore.Score = app.model.Score;

            // Save the highscore into the Highscores .xml file
            TetrisXMLReader.SaveHighscore(app.model.HighscoresFileName, newScore);

            // Display the Highscores UI
            app.Notify(TetrisNotifications.DisplayHighScore, this);
        }

        /// <summary>
        /// Player event when the "Dont Save Score" button has been pressed.
        /// </summary>
        public void OnDontSaveScore()
        {
            // Display the Highscores UI
            app.Notify(TetrisNotifications.DisplayHighScore, this);
        }

        /// <summary>
        /// Displays the highscores loaded from the .xml file, and makes the correct UI elements visible.
        /// </summary>
        private void DisplayHighscores()
        {
            // Load the highscores from the file
            Highscores highscores = TetrisXMLReader.LoadHighscores(app.model.HighscoresFileName);

            // Make highscores UI visible
            view.ScoreContainer.SetActive(true);
            view.InputContainer.SetActive(false);

            // Go over highscores and update the UI elements.
            ScoreView[] scoresView = view.ScoresListContainer.GetComponentsInChildren<ScoreView>();
            for (int i = 0; i < scoresView.Length; i++)
            {
                ScoreView scoreView = scoresView[i];
                if (i >= highscores.Highscore.Count) // If the current index of our UI element is larger than our list of highscores, we make an empty row.
                {
                    scoreView.Name.text = "";
                    scoreView.Score.text = "";
                }
                else
                {
                    Highscore currScore = highscores.Highscore[i];
                    scoreView.Name.text = currScore.Name;
                    scoreView.Score.text = currScore.Score.ToString();
                }
            }
        }

        /// <summary>
        /// Event for when the Level UI element needs to be updated.
        /// </summary>
        /// <param name="newLevel">The new level value.</param>
        private void OnUpdateLevel(int newLevel)
        {
            view.LevelText.text = "Level: " + newLevel;
        }

        /// <summary>
        /// Event for when the Score UI element needs to be updated.
        /// </summary>
        /// <param name="newScore">The new score value.</param>
        private void OnUpdateScore(int newScore)
        {
            view.ScoreText.text = "Score: " + newScore;
        }

        /// <summary>
        /// Event for when the Shape Preview UI element needs to be updated.
        /// </summary>
        /// <param name="newPreview">The shape to be previewed.</param>
        private void OnUpdatePreview(GameObject newPreview)
        {
            // If a previewed object already exists, we destroy it.
            if (model.PreviewedObject != null)
            {
                Destroy(model.PreviewedObject);
            }

            // Creates a clone of the object to be previewed, adjusts its visibility flags and layers.
            model.PreviewedObject = Instantiate(newPreview, view.PreviewContainer.position, Quaternion.identity, view.PreviewContainer);
            model.PreviewedObject.hideFlags = HideFlags.None;
            model.PreviewedObject.SetActive(true);
            model.PreviewedObject.AddComponent<Rotator>();
            SetLayerRecursively(model.PreviewedObject, view.PreviewContainer.gameObject.layer);
        }

        /// <summary>
        /// Event for when the game ends.
        /// </summary>
        private void OnGameOver()
        {
            view.EndingContainer.SetActive(true);
        }

        /// <summary>
        /// Sets a GameObjects and all of it's childrens layers.
        /// </summary>
        /// <param name="objectToChange">The object who's layer is to be changed.</param>
        /// <param name="newLayer">The new layer value.</param>
        private void SetLayerRecursively(GameObject objectToChange, int newLayer)
        {
            objectToChange.layer = newLayer;
            foreach (Transform child in objectToChange.transform)
            {
                SetLayerRecursively(child.gameObject, newLayer);
            }
        }
        #endregion
    }
}