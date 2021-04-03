using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris3D
{
    public static class TetrisNotifications
    {
        public const string ResizeGrid = "game.resize";
        public const string StartGame = "game.start";
        public const string RotateCameraInput = "game.camera.rotate";
        public const string MoveShape = "game.shapes.move";
        public const string ShapeFall = "game.shapes.fall";
        public const string RotateShape = "game.shapes.rotate";
        public const string DropNewShape = "game.shapes.new";
        public const string ShapeStoppedFalling = "game.shapeStopped";
        public const string CheckLayerClear = "game.checkLayer";
        public const string SpeedUpFall = "game.speedUpFall";
        public const string UIUpdateLevel = "game.ui.update.level";
        public const string UIUpdateScore = "game.ui.update.score";
        public const string UIUpdatePreview = "game.ui.update.preview";
        public const string GameOver = "game.over";
        public const string SaveHighscore = "game.over.save";
        public const string DontSaveHighscore = "game.over.dontsave";
        public const string DisplayHighScore = "game.over.highscore";
        public const string RestartGame = "game.restart";
    }
}