using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Tetris3D.Utility;

namespace Tetris3D.Models
{
    public class TetrisModel : TetrisElement
    {
        #region Data
        // Prefabs
        public GameObject CubePrefab;
        public Material PhantomCubeMaterial;

        // Settings
        public string SettingsFileName = "XMLFiles/Settings.xml"; // Path and file name for settings
        public string HighscoresFileName = "XMLFiles/Highscores.xml"; // Path and file name for highscores
        public float FallSpeed; // The speed at which shapes fall
        public float FallSpeedAcceleration; // The acceleration of the fall speed, increased every level.
        public int GridWidth;
        public int GridDepth;
        public int GridHeight;
        public float FallSpeedMultiplier = 1.0f;
        public float FastFallMultiplier = 10.0f;

        // Key: Shape gameobject to clone. Value: Weight of the shape, AKA likelihood of appearing in-game
        public List<KeyValuePair<GameObject, int>> Shapes = new List<KeyValuePair<GameObject, int>>();

        // Game State
        public GameState MatchState = GameState.Start;

        // Game Data
        public int Level = 1;
        public int Score = 0;
        public Transform[,,] GameGrid; // 3-dimensional main game grid. All collision and layer clear calculations are used with this.
        public GameObject CurrentShape; // The active shape being controlled by the player.
        public Queue<GameObject> NextShapes; // Queue containing the next shapes.
        public GameObject PhantomShape; // The phantom shape being displayed under the current shape
        public int BaseScorePerClear = 100; // Base score when a layer is cleared
        public int LayersCleared = 0; // Current amount of layers cleared
        public int LayersToNextLevel { get { return (int)(0.5f * Level * (app.model.Level + 1)); } } // The amount of layers required in total to reach the next level.
        public int ScoreForLayerClear { get { return BaseScorePerClear * (int)Mathf.Pow(2, app.model.Level - 1); } } // The score gained from clearing a layer at the current level.

        // Timers
        public float CurrentFallTimer = 0f; // Timer for shapes to fall.
        #endregion

        #region Unity Methods
        #endregion

        #region Custom Methods
        #endregion
    }
}