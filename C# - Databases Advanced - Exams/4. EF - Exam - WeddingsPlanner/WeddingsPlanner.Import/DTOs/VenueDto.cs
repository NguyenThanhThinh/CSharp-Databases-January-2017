namespace WeddingsPlanner.Import.DTOs
{
    using System.Xml.Serialization;

    [XmlType("venue")]
    class VenueDto
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlElement]
        public int Capacity { get; set; }

        [XmlElement]
        public string Town { get; set; }
    }
}
