namespace MassDefect.App.Export.DTOs
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlType(TypeName = "anomaly")]
    public class AnomalyWithVictimsDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("origin-planet")]
        public string OriginPlanet { get; set; }

        [XmlAttribute("teleport-planet")]
        public string TeleportPlanet { get; set; }

        [XmlArray(ElementName = "victims")]
        [XmlArrayItem(ElementName = "victim")]
        public List<VictimDto> Victims { get; set; }
    }
}