namespace MassDefect.App.Export.DTOs
{
    using System.Xml.Serialization;
    using System.Collections.Generic;

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
