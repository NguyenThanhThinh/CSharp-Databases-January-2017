namespace PlanetHunters.Export.exportDtos
{
    using System.Xml.Serialization;

    [XmlType("Astronomer")]
    public class AstronomerPioneerDto
    {
        [XmlAttribute("Pioneer")]
        public bool Pioneer { get; set; }

        [XmlText]
        public string Name { get; set; }
    }
}