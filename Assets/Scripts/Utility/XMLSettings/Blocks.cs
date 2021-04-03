using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

namespace Tetris3D.Utility
{
    [XmlRoot(ElementName = "Blocks")]
    public class Blocks
    {
        [XmlElement(ElementName = "Block")]
        public List<Block> Block { get; set; }
    }
}