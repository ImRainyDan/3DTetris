using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using Tetris3D.Utility;
using Tetris3D.View;
using Tetris3D;

namespace Tetris3D.Controllers
{
    public class TetrisController : Controller
    {
        #region Data
        
        #endregion

        #region Unity Methods
        private void Awake()
        {
            LoadDataSettingsFromFile(); // Load game data settings from XML file. All model data comes from this file.
            InitializeGameLogicVariables(); // Initialize variables and prepares game for start.
        }

        private void Start()
        {
            StartGame();
        }

        private void Update()
        {
            switch (app.model.MatchState)
            {
                case GameState.InProgress:
                    UpdateFallingShapes(); // Main physics update, causing the current active shape to fall downwards.
                    break;
            }
        }
        #endregion

        #region Custom Methods
        public override void OnNotification(string eventPath, Object eventSource, params object[] eventData)
        {
            switch (eventPath)
            {
                case TetrisNotifications.ShapeStoppedFalling:
                    GameObject stoppedObject = (GameObject)eventData[0];

                    OnFallStopped(stoppedObject);
                    break;

                case TetrisNotifications.GameOver:
                    app.model.MatchState = GameState.EndingSaveScore;
                    break;

                case TetrisNotifications.DisplayHighScore:
                    app.model.MatchState = GameState.EndingViewScore;
                    break;

                case TetrisNotifications.RestartGame:
                    SceneManager.LoadScene(0);
                    break;
            }
        }

        /// <summary>
        /// Loads model game data from the settings .xml file.
        /// </summary>
        public void LoadDataSettingsFromFile()
        {
            // Load and read the Settings .xml file
            TetrisSettings settings = TetrisXMLReader.LoadSettings(app.model.SettingsFileName);

            if (settings != null)
            {

                // Initializing game settings
                app.model.FallSpeed = settings.GameSettings.InitialFallSpeed;
                app.model.FallSpeedAcceleration = settings.GameSettings.FallSpeedAcceleration;

                // Initializing grid settings
                app.model.GridWidth = settings.GridSettings.GridWidth;
                app.model.GridDepth = settings.GridSettings.GridDepth;
                app.model.GridHeight = settings.GridSettings.GridHeight;

                // Initializing shapes settings
                foreach (KeyValuePair<GameObject, int> shape in app.model.Shapes)
                {
                    Destroy(shape.Key);
                }

                app.model.Shapes.Clear();

                // Read each shape in the file and create an object for it
                foreach (Shape s in settings.ShapesSettings.Shape)
                {
                    // Create a new shape object and hide it from the scene and hierarchy. This object will be cloned when used in the actual game.
                    GameObject shapeObject = new GameObject("Shape");
                    shapeObject.hideFlags = HideFlags.HideInHierarchy;
                    shapeObject.SetActive(false);

                    // Parse color setting from hex to Color
                    Color cubeColor;
                    ColorUtility.TryParseHtmlString(s.Color, out cubeColor);

                    // Go over each block in the shape and add it to the shape object.
                    foreach (Block b in s.Blocks.Block)
                    {
                        GameObject cubeObject = Instantiate(app.model.CubePrefab, shapeObject.transform);
                        cubeObject.transform.localPosition = new Vector3(b.X, b.Y, b.Z);

                        // Set the shapes color
                        Renderer cubeRenderer = cubeObject.GetComponent<Renderer>();
                        if (cubeRenderer != null)
                        {
                            cubeRenderer.material.SetColor("_Color", cubeColor);
                        }
                    }

                    // Add the shape object to our list of shapes, and save its weight (probability of appearing) for later use
                    app.model.Shapes.Add(new KeyValuePair<GameObject, int>(shapeObject, s.Weight));
                }
            }

            // Resize the grid view
            ResizeGrid(app.model.GridWidth, app.model.GridHeight, app.model.GridDepth);
        }


        /// <summary>
        /// Selects a weighted random shape from all the possible shape options in the model.
        /// </summary>
        /// <returns>A weighted random shape</returns>
        public GameObject PickRandomShape()
        {
            GameObject randomShape = null;
            int totalWeight = 0;

            // Create a new key/value list containing the game object and their new cumulative weight
            List<KeyValuePair<GameObject, int>> weightedPairs = new List<KeyValuePair<GameObject, int>>();
            foreach (KeyValuePair<GameObject, int> currentShape in app.model.Shapes)
            {
                weightedPairs.Add(new KeyValuePair<GameObject, int>(currentShape.Key, currentShape.Value + totalWeight));
                totalWeight += currentShape.Value;
            }

            // Sort the weights from smallest to biggest
            weightedPairs.Sort((x, y) => { return x.Value.CompareTo(y.Value); });

            // Pick a random weighted gameobject
            float randomValue = Random.Range(0.0f, totalWeight);
            foreach (KeyValuePair<GameObject, int> currentPair in weightedPairs)
            {
                if (randomValue < currentPair.Value)
                {
                    randomShape = currentPair.Key;
                    break;
                }
            }

            return randomShape;
        }

        /// <summary>
        /// Returns true if the shape in it's current position is colliding with any other block on the grid.
        /// </summary>
        /// <param name="objectToCheck">The GameObject to check collisions on.</param>
        /// <returns>Is the object colliding with another object in the grid?</returns>
        public bool IsGameObjectCollidingWithGrid(GameObject objectToCheck)
        {
            bool result = false;
            Vector3Int currPos = Vector3Int.RoundToInt(objectToCheck.transform.position);

            if (IsGameObjectInGridArray(objectToCheck) && app.model.GameGrid[currPos.x, currPos.y, currPos.z] != null)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Returns true if the shape in it's current position is located within the main game grid.
        /// </summary>
        /// <param name="objectToCheck">The GameObject to check it's position.</param>
        /// <returns>Is the object within the bounds of the main grid?</returns>
        public bool IsGameObjectInGridArray(GameObject objectToCheck)
        {
            bool isInBounds = false;
            Vector3Int currPos = Vector3Int.RoundToInt(objectToCheck.transform.position);

            if (currPos.x >= 0 && currPos.x < app.model.GameGrid.GetLength(0) &&
                currPos.y >= 0 && currPos.y < app.model.GameGrid.GetLength(1) &&
                currPos.z >= 0 && currPos.z < app.model.GameGrid.GetLength(2))
            {
                isInBounds = true;
            }

            return isInBounds;
        }

        /// <summary>
        /// Returns true if the shape in it's current position is located within the main game grids bounds, excluding the top.
        /// </summary>
        /// <param name="objectToCheck">The GameObject to check it's position.</param>
        /// <returns>Is the object within the bounds of the main grid, excluding the top?</returns>
        public bool IsGameObjectInGridBounds(GameObject objectToCheck)
        {
            bool isInBounds = false;
            Vector3Int currPos = Vector3Int.RoundToInt(objectToCheck.transform.position);

            if (currPos.x >= 0 && currPos.x < app.model.GridWidth &&
                currPos.z >= 0 && currPos.z < app.model.GridDepth &&
                currPos.y >= 0)
            {
                isInBounds = true;
            }

            return isInBounds;
        }

        /// <summary>
        /// Returns true if the shape is located outside the grids max height, resulting in a gameover.
        /// </summary>
        /// <param name="objectToCheck">The GameObject to check it's position.</param>
        /// <returns>Is the game over?</returns>
        private bool IsGameOver(GameObject objectToCheck)
        {
            bool isGameover = false;
            foreach (Transform child in objectToCheck.transform)
            {
                Vector3Int currPos = Vector3Int.RoundToInt(child.position);

                if (currPos.y >= app.model.GridHeight)
                {
                    isGameover = true;
                    break;
                }
            }

            return isGameover;
        }

        /// <summary>
        /// Initialize variables and prepares game for start.
        /// </summary>
        private void InitializeGameLogicVariables()
        {
            app.model.GameGrid = new Transform[app.model.GridWidth, app.model.GridHeight, app.model.GridDepth];
            app.model.NextShapes = new Queue<GameObject>();
            app.model.NextShapes.Enqueue(PickRandomShape());
            app.model.Level = 1;
            app.model.Score = 0;
            app.model.LayersCleared = 0;

            // Update the UI
            app.Notify(TetrisNotifications.UIUpdateLevel, this, app.model.Level);
            app.Notify(TetrisNotifications.UIUpdateScore, this, app.model.Score);
        }

        /// <summary>
        /// Main physics loop for falling blocks.
        /// </summary>
        private void UpdateFallingShapes()
        {
            // The timer for the current shape to fall is sped up by the FallSpeed Multiplier.
            app.model.CurrentFallTimer += Time.deltaTime * app.model.FallSpeedMultiplier;

            if (app.model.CurrentFallTimer >= 1.0f / app.model.FallSpeed)
            {
                // Notifies the ShapesController to make the current active shape fall downwards one tile.
                app.Notify(TetrisNotifications.ShapeFall, this);

                app.model.CurrentFallTimer -= 1.0f / app.model.FallSpeed;
            }
        }

        /// <summary>
        /// Clears an entire given layer of blocks, and updates the score and layers cleared variables.
        /// </summary>
        /// <param name="layer">The layer to clear.</param>
        private void ClearLayer(int layer)
        {
            // Clear the layer, remove blocks from grid and view
            for (int xx = 0; xx < app.model.GridWidth; xx++)
            {
                for (int zz = 0; zz < app.model.GridDepth; zz++)
                {
                    Transform block = app.model.GameGrid[xx, layer, zz];

                    app.model.GameGrid[xx, layer, zz] = null;
                    Destroy(block.gameObject);
                }
            }

            app.model.Score += app.model.ScoreForLayerClear;
            app.model.LayersCleared++;
        }

        /// <summary>
        /// Returns all layers in the grid that need to be cleared due to them being entirely filled up.
        /// Will only check layers that the last object placed is located in.
        /// </summary>
        /// <param name="objectThatWasPlaced">The last object to be placed in the grid.</param>
        /// <returns>A list of layers that need to be cleared.</returns>
        private List<int> GetLayersToClear(GameObject objectThatWasPlaced)
        {
            List<int> heightLayersToCheck = new List<int>();
            List<int> layersToClear = new List<int>();

            // Creates a list of layers that need to be checked. In order to improve performance, we only check layers that the last object placed is located in.
            foreach (Transform child in objectThatWasPlaced.transform)
            {
                Vector3Int childPos = Vector3Int.RoundToInt(child.position);
                if (!heightLayersToCheck.Contains(childPos.y))
                {
                    heightLayersToCheck.Add(childPos.y);
                }
            }

            // Goes over all layers that need to be checked, and determines whether they are full or not.
            foreach (int layer in heightLayersToCheck)
            {
                bool isLayerFilled = true;
                for (int xx = 0; xx < app.model.GridWidth; xx++)
                {
                    for (int zz = 0; zz < app.model.GridDepth; zz++)
                    {
                        if (app.model.GameGrid[xx, layer, zz] == null)
                        {
                            isLayerFilled = false;
                            break;
                        }
                    }
                }

                // If the layer is full, it needs to be cleared and so we add it to our list.
                if (isLayerFilled)
                {
                    layersToClear.Add(layer);
                }
            }

            // We sort the layers list from smallest to biggest, so that we can easily determine the highest and lowest layers.
            layersToClear.Sort((x, y) => { return x.CompareTo(y); });

            return layersToClear;
        }

        /// <summary>
        /// Shifts all layers, from a given starting layer, downwards by the amount received as an argument.
        /// </summary>
        /// <param name="startingLayer">The layer to start shifting downwards from.</param>
        /// <param name="amount">The size of the shift.</param>
        private void ShiftLayersDown(int startingLayer, int amount)
        {
            List<Transform> shapesMoved = new List<Transform>();

            // We only want to shift layers that are our starting layer and above, so we go over all layers starting from the starting layer, until we reach the top of the grid.
            for (int yy = startingLayer; yy < app.model.GridHeight; yy++)
            {
                for (int xx = 0; xx < app.model.GridWidth; xx++)
                {
                    for (int zz = 0; zz < app.model.GridDepth; zz++)
                    {
                        Transform block = app.model.GameGrid[xx, yy, zz];
                        if (block != null)
                        {
                            // Moves the block at it's current position downwards by "amount". 
                            app.model.GameGrid[xx, yy, zz] = null;
                            app.model.GameGrid[xx, yy - amount, zz] = block;
                            block.position += new Vector3(0, -amount, 0);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Resizes the grid view.
        /// </summary>
        /// <param name="width">Grid width.</param>
        /// <param name="height">Grid height.</param>
        /// <param name="depth">Grid depth.</param>
        private void ResizeGrid(int width, int height, int depth)
        {
            app.view.GridContainer.transform.localScale = new Vector3(width, height, depth);

            // Position the grid for easier calculations of block positions
            app.view.GridContainer.transform.localPosition = new Vector3(width / 2.0f - 0.5f, height / 2.0f - 0.5f, depth / 2.0f - 0.5f); 
        }

        /// <summary>
        /// Performs all the necessary functions to start the game.
        /// </summary>
        private void StartGame()
        {
            app.model.MatchState = GameState.InProgress;
            app.Notify(TetrisNotifications.DropNewShape, this); // Start the game off by dropping a shape onto the grid.
        }

        /// <summary>
        /// Event for when a shape has stopped falling.
        /// </summary>
        /// <param name="stoppedShape">The shape that has stopped falling.</param>
        private void OnFallStopped(GameObject stoppedShape)
        {
            bool wasObjectPlaced = PlaceObjectOnGrid(stoppedShape); // Officially add the object to the main grid array.

            if (wasObjectPlaced)
            {
                // If the object was placed, we check if any layers need to be cleared, clears them, and updates our level/score.
                List<int> layersCleared = GetLayersToClear(stoppedShape);
                if (layersCleared.Count > 0)
                {
                    foreach (int layerToClear in layersCleared)
                    {
                        ClearLayer(layerToClear);

                        // Check if the player has leveled up.
                        while (app.model.LayersCleared >= app.model.LayersToNextLevel)
                        {
                            app.model.Level++;
                            app.model.FallSpeed += (app.model.Level - 1) * app.model.FallSpeedAcceleration; // Shapes fall faster when the level increases.
                        }
                    }

                    // Update Score/Level UI
                    app.Notify(TetrisNotifications.UIUpdateLevel, this, app.model.Level);
                    app.Notify(TetrisNotifications.UIUpdateScore, this, app.model.Score);

                    // After layers are cleared, we want to shift all layers above the highest layer cleared downwards.
                    // The highest layer  that was cleared is the last value in the layersCleared array.
                    ShiftLayersDown(layersCleared[layersCleared.Count - 1] + 1, layersCleared.Count);
                }

                app.Notify(TetrisNotifications.DropNewShape, this);
            }
            else
            {
                // If the object could not be placed on the grid, it probably means it's out of bounds, so we check if the game is supposed to end.
                if (IsGameOver(stoppedShape))
                {
                    app.Notify(TetrisNotifications.GameOver, this);
                }
            }
        }

        /// <summary>
        /// Places a shape on the main grid array. This allows for collision calculation and main gameplay checks to occur.
        /// </summary>
        /// <param name="objectToPlace">The shape to be placed onto the grid.</param>
        /// <returns>Has the shapes been successfully placed on the grid?</returns>
        private bool PlaceObjectOnGrid(GameObject objectToPlace)
        {
            bool wasPlacedSuccessfully = true;
            Transform[,,] newGrid = (Transform[,,]) app.model.GameGrid.Clone(); // Clone the grid and attempt to place the new shape into the clone

            foreach (Transform child in objectToPlace.transform)
            {
                Vector3Int newPos = Vector3Int.RoundToInt(child.position);

                // If the current block is outside the grid array, or the current position is occupied, the object placement has failed.
                if (!IsGameObjectInGridArray(child.gameObject) || newGrid[newPos.x, newPos.y, newPos.z] != null)
                {
                    wasPlacedSuccessfully = false;
                    break;
                }
                else
                {
                    // Place the block in it's proper position on the grid.
                    newGrid[newPos.x, newPos.y, newPos.z] = child;
                }
            }
            
            // If the object was successfully placed into the clone grid, we update our main game grid.
            if (wasPlacedSuccessfully)
            {
                app.model.GameGrid = newGrid;
            }

            return wasPlacedSuccessfully;
        }
        #endregion
    }
}