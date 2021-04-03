using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace Tetris3D.Utility
{
    /// <summary>
    /// Static class used for writing and reading from the .xml data files.
    /// </summary>
    public static class TetrisXMLReader
    {
        /// <summary>
        /// Reads from the Settings .xml file and returns its data.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <returns>The settings of the game.</returns>
        public static TetrisSettings LoadSettings(string path)
        {
            path = Path.Combine(Application.streamingAssetsPath, path);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TetrisSettings));
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    return serializer.Deserialize(stream) as TetrisSettings;
                }
            }
            catch (Exception e)
            {
                Debug.Log("Exception loading settings file: " + e);

                return null;
            }
        }

        /// <summary>
        /// Reads from the Highscores .xml file and returns its data.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <returns>The highscores of the game.</returns>
        public static Highscores LoadHighscores(string path)
        {
            path = Path.Combine(Application.streamingAssetsPath, path);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Highscores));
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    return serializer.Deserialize(stream) as Highscores;
                }
            }
            catch (Exception e)
            {
                Debug.Log("Exception loading highscores file: " + e);

                return null;
            }
        }

        /// <summary>
        /// Writes highscores into the Highscores .xml file.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <param name="newScore">The new highscore to save.</param>
        public static void SaveHighscore(string path, Highscore newScore)
        {
            path = Path.Combine(Application.streamingAssetsPath, path);

            // Retreives the current highscores from the file.
            Highscores currentHighscores = LoadHighscores(path);

            if (currentHighscores != null)
            {
                // Adds the new highscore to the list and sorts it from highest to lowest.
                currentHighscores.Highscore.Add(newScore);
                currentHighscores.Highscore.Sort((x, y) => { return y.Score.CompareTo(x.Score); });

                // Trim the list if there are more than 10 highscores.
                if (currentHighscores.Highscore.Count > 10)
                {
                    currentHighscores.Highscore = currentHighscores.Highscore.GetRange(0, 10); // Get only top 10
                }

                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Highscores));
                    using (FileStream stream = new FileStream(path, FileMode.Open))
                    {
                        serializer.Serialize(stream, currentHighscores);
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("Exception saving highscores file: " + e);
                }
            }
        }
    }
}