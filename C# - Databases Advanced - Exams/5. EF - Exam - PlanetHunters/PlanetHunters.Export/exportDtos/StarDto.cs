namespace PlanetHunters.Export.exportDtos
{   
    using System.Xml.Serialization;

    [XmlType("Star")]
    public class StarDto
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Temperature")]
        public int Temperature { get; set; }

        [XmlElement("StarSystem")]
        public string StarSystem { get; set; }

        [XmlElement("DiscoveryInfo")]
        public DiscoveryInfoDto DiscoveryInfo { get; set; }
    }
}
