namespace WeddingsPlanner.Export.DTOs
{
    using System.Xml.Serialization;

    [XmlType("guest")]
    public class GuestDto
    {
        [XmlAttribute("family")]
        public string Family { get; set; }

        [XmlText]
        public string Name { get; set; }
    }
}
