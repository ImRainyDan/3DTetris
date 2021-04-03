using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris3D.Utility
{
    [XmlRoot(ElementName = "Highscore")]
    public class Highscore
    {
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "Score")]
        public int Score { get; set; }
    }
}