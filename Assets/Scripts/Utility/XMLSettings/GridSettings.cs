using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

namespace Tetris3D.Utility
{
    [XmlRoot(ElementName = "GridSettings")]
    public class GridSettings
    {
        [XmlElement(ElementName = "GridWidth")]
        public int GridWidth { get; set; }
        [XmlElement(ElementName = "GridDepth")]
        public int GridDepth { get; set; }
        [XmlElement(ElementName = "GridHeight")]
        public int GridHeight { get; set; }
    }
}