using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

namespace Tetris3D.Utility
{
    [XmlRoot(ElementName = "ShapesSettings")]
    public class ShapesSettings
    {
        [XmlElement(ElementName = "Shape")]
        public List<Shape> Shape { get; set; }
    }
}