namespace WeddingsPlanner.Export.DTOs
{
    using System.Xml.Serialization;

    [XmlType("venue")]
    public class VenueDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("capacity")]
        public int Capacity { get; set; }

        [XmlElement("weddings-count")]
        public int WeddingsCount { get; set; }
    }
}