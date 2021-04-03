using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris3D.Components
{
    public class BlinkText : MonoBehaviour
    {
        #region Data
        public float Frequency = 1; // How many times per second is the renderer enabled/disabled

        private Text mainRenderer;
        private float currentTime = 0;
        #endregion

        #region Unity Methods
        private void Start()
        {
            mainRenderer = GetComponent<Text>();
        }

        private void Update()
        {
            if (mainRenderer != null)
            {
                currentTime += Time.deltaTime;

                if (currentTime >= Frequency)
                {
                    mainRenderer.enabled = !mainRenderer.enabled;

                    currentTime -= Frequency;
                }
            }
        }
        #endregion
    }
}