using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris3D.Controllers
{
    public abstract class Controller : TetrisElement
    {
        public abstract void OnNotification(string eventPath, Object eventSource, params object[] eventData);
    }
}