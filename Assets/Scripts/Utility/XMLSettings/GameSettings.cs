using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

namespace Tetris3D.Utility
{
    [XmlRoot(ElementName = "GameSettings")]
    public class GameSettings
    {
        [XmlElement(ElementName = "InitialFallSpeed")]
        public float InitialFallSpeed { get; set; }
        [XmlElement(ElementName = "FallSpeedAcceleration")]
        public float FallSpeedAcceleration { get; set; }
    }
}