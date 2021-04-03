using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

namespace Tetris3D.Utility
{
    [XmlRoot(ElementName = "Block")]
    public class Block
    {
        [XmlElement(ElementName = "X")]
        public int X { get; set; }
        [XmlElement(ElementName = "Y")]
        public int Y { get; set; }
        [XmlElement(ElementName = "Z")]
        public int Z { get; set; }
    }
}