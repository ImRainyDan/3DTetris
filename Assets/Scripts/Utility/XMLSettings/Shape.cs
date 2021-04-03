using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

namespace Tetris3D.Utility
{
    [XmlRoot(ElementName = "Shape")]
    public class Shape
    {
        [XmlElement(ElementName = "Blocks")]
        public Blocks Blocks { get; set; }

        [XmlElement(ElementName = "Color")]
        public string Color { get; set; }

        [XmlElement(ElementName = "Weight")]
        public int Weight { get; set; }
    }
}