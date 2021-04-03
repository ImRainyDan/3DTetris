using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris3D.Components
{
    /// <summary>
    /// This class will stretch the tiling of the main material's shader to fit the scale of the gameobject.
    /// </summary>
    [ExecuteAlways]
    public class ShaderTilingStretch : MonoBehaviour
    {
        #region Data
        private Renderer mainRenderer;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            mainRenderer = GetComponent<Renderer>();
        }

        private void Update()
        {
            if (mainRenderer != null)
            {
                mainRenderer.sharedMaterial.mainTextureScale = new Vector2(transform.lossyScale.x, transform.lossyScale.y);
            }
        }
        #endregion
    }

}