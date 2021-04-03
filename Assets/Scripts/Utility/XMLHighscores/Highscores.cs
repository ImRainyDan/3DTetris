using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris3D.Utility
{
    [XmlRoot(ElementName = "Highscores")]
    public class Highscores
    {
        [XmlElement(ElementName = "Highscore")]
        public List<Highscore> Highscore { get; set; }
    }
}