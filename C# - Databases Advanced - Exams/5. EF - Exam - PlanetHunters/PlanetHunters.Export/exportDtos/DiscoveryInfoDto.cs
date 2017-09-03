namespace PlanetHunters.Export.exportDtos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Xml.Serialization;

    [XmlType("DiscoveryInfo")]
    public class DiscoveryInfoDto
    {
        [XmlAttribute("DiscoveryDate")]
        [Column(TypeName = "DATE")]
        public DateTime DiscoveryDate { get; set; }

        [XmlAttribute("TelescopeName")]
        public string TelescopeName { get; set; }

        [XmlElement("Astronomer")]
        public List<AstronomerPioneerDto> AsronomerPionersDto { get; set; }
    }
}