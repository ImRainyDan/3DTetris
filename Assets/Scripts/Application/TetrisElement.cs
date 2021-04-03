using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris3D
{
    public abstract class TetrisElement : MonoBehaviour
    {
        public TetrisApplication app {
            get
            {
                if (_app == null)
                {
                    _app = GameObject.FindObjectOfType<TetrisApplication>();
                }

                return _app;
            }
        }

        private TetrisApplication _app;
    }
}