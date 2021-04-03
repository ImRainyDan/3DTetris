using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tetris3D;

namespace Tetris3D.Controllers
{
    public class ShapesController : Controller
    {
        #region Data

        #endregion

        #region Unity Methods

        #endregion

        #region Custom Methods
        public override void OnNotification(string eventPath, Object eventSource, params object[] eventData)
        {
            switch (eventPath)
            {
                case TetrisNotifications.SpeedUpFall:
                    bool isFallSpedUp = (bool)eventData[0];

                    if (app.model.MatchState == GameState.InProgress)
                    {
                        if (isFallSpedUp)
                        {
                            app.model.FallSpeedMultiplier = app.model.FastFallMultiplier; // Speed up fall
                        }
                        else
                        {
                            app.model.FallSpeedMultiplier = 1.0f; // Reset speed
                        }
                    }
                    break;

                case TetrisNotifications.MoveShape:
                    Vector3 axisMovement = (Vector3)eventData[0];

                    if (app.model.MatchState == GameState.InProgress)
                    {
                        if (axisMovement != Vector3.zero)
                        {
                            OnMoveShapeInput(axisMovement);
                        }
                    }
                    break;

                case TetrisNotifications.RotateShape:
                    Vector3 axisRotation = (Vector3)eventData[0];

                    if (app.model.MatchState == GameState.InProgress)
                    {
                        if (axisRotation != Vector3.zero)
                        {
                            OnRotateShapeInput(axisRotation);
                        }
                    }
                    break;

                case TetrisNotifications.ShapeFall:
                    ShapeFall();
                    break;

                case TetrisNotifications.DropNewShape:
                    DropNewShape();
                    break;
            }
        }

        /// <summary>
        /// Called in the main physics loop, this makes the current shape controlled by the player fall 1 tile.
        /// If it can't move 1 tile down, then it has hit the bottom, and notifies the main controller.
        /// </summary>
        private void ShapeFall()
        {
            Vector3 fallDirection = Vector3.down;

            if (CanMoveInDirection(app.model.CurrentShape, fallDirection))
            {
                app.model.CurrentShape.transform.position += fallDirection;
            }
            else
            {
                app.Notify(TetrisNotifications.ShapeStoppedFalling, this, app.model.CurrentShape);
            }
        }

        /// <summary>
        /// Selects the next shape in the Shapes Queue to become the current active shape the player controls, and instantiates it.
        /// It enqueues a new random shape into the Shapes Queue, updates UI elements and resets main game logic timers.
        /// </summary>
        /// <returns>The new shape being dropped</returns>
        private GameObject DropNewShape()
        {
            // New shapes always drop at the top-center of the grid
            Vector3 newPosition = new Vector3((int)(app.model.GridWidth / 2.0f), (int)(app.model.GridHeight), (int)(app.model.GridDepth / 2.0f));

            // Instantiates the next shape in the queue and initializes it's GameObject flags
            GameObject nextShape = app.model.NextShapes.Dequeue();
            GameObject newShape = Instantiate(nextShape, newPosition, Quaternion.identity, app.view.Shapes.transform);
            newShape.hideFlags = HideFlags.None;
            newShape.SetActive(true);

            // If a current active shape already exists, detach all the blocks tied to the shape and then delete the shape container
            if (app.model.CurrentShape != null)
            {
                while (app.model.CurrentShape.transform.childCount > 0)
                {
                    app.model.CurrentShape.transform.GetChild(0).SetParent(app.view.Shapes.transform);
                }
                Destroy(app.model.CurrentShape);
            }

            app.model.CurrentShape = newShape; // New current shape assigned
            app.model.CurrentFallTimer = 0; // Resetting main shape fall timer

            // Enqueues a new random shape and updates the UI
            GameObject upcomingShape = app.controller.PickRandomShape();
            app.Notify(TetrisNotifications.UIUpdatePreview, this, upcomingShape);
            app.model.NextShapes.Enqueue(upcomingShape);

            DisplayPhantomShape(newShape); // Whenever a shape is created, moved or rotated, we want to display the "phantom" shape beneath it.

            return newShape;
        }

        /// <summary>
        /// Player input event when a shape needs to be rotated.
        /// </summary>
        /// <param name="axisRotation">The axis of rotation</param>
        private void OnRotateShapeInput(Vector3 axisRotation)
        {
            if (app.model.MatchState == GameState.InProgress)
            {
                if (axisRotation != Vector3.zero)
                {
                    // Rotates the current active shape in the axis given, relative to the current camera angle factor
                    RotateShapeOnAxis(app.model.CurrentShape, Quaternion.AngleAxis(app.camController.GetCameraAngleFactor(), Vector3.up) * axisRotation);
                }
            }
        }

        /// <summary>
        /// Rotates a given tetris shape relative to an axis.
        /// </summary>
        /// <param name="shape">The shape to be rotated</param>
        /// <param name="axis">The axis of rotation</param>
        /// <returns>Did the shape rotate successfully?</returns>
        private bool RotateShapeOnAxis(GameObject shape, Vector3 axis)
        {
            bool hasRotated = false;
            Vector3 originalPos = shape.transform.position;

            // Checks if the shape can be rotated in its current position without colliding with anything, and rotates it if it can.
            if (CanRotateInAxis(shape, axis))
            {
                shape.transform.Rotate(axis, 90, Space.World);
                hasRotated = true;
            }
            else
            {
                // Try rotating again by shifting the shape forward. If the shape can be rotated in its new position without colliding with anything, it rotates it.
                shape.transform.position = originalPos + Vector3.forward;
                if (CanRotateInAxis(shape, axis))
                {
                    shape.transform.Rotate(axis, 90, Space.World);
                    hasRotated = true;
                }
                else
                {
                    // Try rotating again by shifting the shape backwards. If the shape can be rotated in its new position without colliding with anything, it rotates it.
                    shape.transform.position = originalPos + Vector3.back;
                    if (CanRotateInAxis(shape, axis))
                    {
                        shape.transform.Rotate(axis, 90, Space.World);
                        hasRotated = true;
                    }
                    else
                    {
                        // Try rotating again by shifting the shape left. If the shape can be rotated in its new position without colliding with anything, it rotates it.
                        shape.transform.position = originalPos + Vector3.left;
                        if (CanRotateInAxis(shape, axis))
                        {
                            shape.transform.Rotate(axis, 90, Space.World);
                            hasRotated = true;
                        }
                        else
                        {
                            // Try rotating again by shifting the shape right. If the shape can be rotated in its new position without colliding with anything, it rotates it.
                            shape.transform.position = originalPos + Vector3.right;
                            if (CanRotateInAxis(shape, axis))
                            {
                                shape.transform.Rotate(axis, 90, Space.World);
                                hasRotated = true;
                            }
                        }
                    }
                }
            }

            if (hasRotated)
            {
                DisplayPhantomShape(shape);  // Whenever a shape is created, moved or rotated, we want to display the "phantom" shape beneath it.
            }
            else
            {
                shape.transform.position = originalPos; // Reset position when all else fails.
            }

            return hasRotated;
        }

        /// <summary>
        /// Checks if the given shape can be rotated in an axis and not collide with any other blocks or the grid.
        /// </summary>
        /// <param name="shape">The shape to be rotated</param>
        /// <param name="axis">The axis of rotation</param>
        /// <returns>Can the shape rotate in this axis without collision?</returns>
        private bool CanRotateInAxis(GameObject shape, Vector3 axis)
        {
            bool canRotate = true;

            // Simulates the object rotation
            shape.transform.Rotate(axis, 90, Space.World);

            // Goes over all blocks in the shape and checks if it's within bounds and not colliding with any other block.
            foreach (Transform child in shape.transform)
            {
                if (!app.controller.IsGameObjectInGridBounds(child.gameObject) || app.controller.IsGameObjectCollidingWithGrid(child.gameObject))
                {
                    canRotate = false;
                    break;
                }
            }

            // Resets the rotation back to the original
            shape.transform.Rotate(axis, -90, Space.World);

            return canRotate;
        }

        /// <summary>
        /// Player input event when a shape needs to be moved.
        /// </summary>
        /// <param name="axisMovement">The axis of movement</param>
        private void OnMoveShapeInput(Vector3 axisMovement)
        {
            if (app.model.MatchState == GameState.InProgress)
            {
                if (axisMovement != Vector3.zero)
                {
                    // Moves the current active shape in the axis given, relative to the current camera angle factor
                    axisMovement = Quaternion.AngleAxis(app.camController.GetCameraAngleFactor(), Vector3.up) * axisMovement;

                    if (axisMovement.x != 0)
                    {
                        MoveShapeInDirection(app.model.CurrentShape, new Vector3(axisMovement.x, 0, 0));
                    }
                    if (axisMovement.z != 0)
                    {
                        MoveShapeInDirection(app.model.CurrentShape, new Vector3(0, 0, axisMovement.z));
                    }
                }
            }
        }

        /// <summary>
        /// Moves a given tetris shape in a direction
        /// </summary>
        /// <param name="shape">The shape to be moved</param>
        /// <param name="direction">The direction of movement</param>
        /// <returns>Did the shape move successfully?</returns>
        private bool MoveShapeInDirection(GameObject shape, Vector3 direction)
        {
            bool hasMoved = false;

            if (CanMoveInDirection(shape, direction))
            {
                shape.transform.position += direction;
                DisplayPhantomShape(shape); // Whenever a shape is created, moved or rotated, we want to display the "phantom" shape beneath it.

                hasMoved = true;
            }

            return hasMoved;
        }

        /// <summary>
        /// Checks if the given shape can be moved  in a direction and not collide with any other blocks or the grid.
        /// </summary>
        /// <param name="shape">The shape to be moved</param>
        /// <param name="direction">The direction of movement</param>
        /// <returns>Can the shape move in this direction without collision?</returns>
        private bool CanMoveInDirection(GameObject shape, Vector3 direction)
        {
            bool canMove = true;

            // Simulates the object rotation
            shape.transform.position += direction;

            // Goes over all blocks in the shape and checks if it's within bounds and not colliding with any other block.
            foreach (Transform child in shape.transform)
            {
                if (!app.controller.IsGameObjectInGridBounds(child.gameObject) || app.controller.IsGameObjectCollidingWithGrid(child.gameObject))
                {
                    canMove = false;
                    break;
                }
            }

            // Resets shape position back to the original
            shape.transform.position -= direction;

            return canMove;
        }

        /// <summary>
        /// Displays a phantom shape beneath the shape passed as an argument. Designed to show the player where the shape is going to fall.
        /// </summary>
        /// <param name="originalShape">The shape to display it's phantom</param>
        private void DisplayPhantomShape(GameObject originalShape)
        {
            // If a phantom shape exists, destroy it
            if (app.model.PhantomShape != null)
            {
                Destroy(app.model.PhantomShape);
            }

            // Instantiate a clone of the original shape, and adjust it's shader/material to the proper transparency and color.
            app.model.PhantomShape = Instantiate(originalShape, originalShape.transform.position, originalShape.transform.rotation, originalShape.transform.parent);
            foreach (Transform child in app.model.PhantomShape.transform)
            {
                child.localScale = Vector3.one;
                Renderer childRenderer = child.gameObject.GetComponent<Renderer>();
                if (childRenderer != null)
                {
                    Color currentColor = childRenderer.material.GetColor("_Color");
                    childRenderer.material = app.model.PhantomCubeMaterial;
                    childRenderer.material.SetColor("_Color", currentColor);
                }
            }

            Vector3 fallDirection = Vector3.down;

            // Keep trying to move the phantom shape downwards, until it can no longer move down.
            while (CanMoveInDirection(app.model.PhantomShape, fallDirection))
            {
                app.model.PhantomShape.transform.position += fallDirection;
            }

            // If the phantom shapes position is the same as the original shapes position, there is no need for the phantom shape so we destroy it.
            if (app.model.CurrentShape.transform.position == app.model.PhantomShape.transform.position)
            {
                Destroy(app.model.PhantomShape);
            }
        }
        #endregion
    }
}