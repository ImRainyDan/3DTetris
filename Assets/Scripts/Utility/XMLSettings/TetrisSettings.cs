using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

namespace Tetris3D.Utility
{
    [XmlRoot(ElementName = "TetrisSettings")]
    public class TetrisSettings
    {
        [XmlElement(ElementName = "GameSettings")]
        public GameSettings GameSettings { get; set; }

        [XmlElement(ElementName = "GridSettings")]
        public GridSettings GridSettings { get; set; }

        [XmlElement(ElementName = "ShapesSettings")]
        public ShapesSettings ShapesSettings { get; set; }
    }
}