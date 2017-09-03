namespace WeddingsPlanner.Import.DTOs
{
    using Models.Enums;
    using System.Xml.Serialization;

    [XmlType("present")]
    public class PresentDto
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("invitation-id")]
        public int InvitationId { get; set; }

        [XmlAttribute("present-name")]
        public string PresentName { get; set; }

        [XmlAttribute("size")]
        public PresentSize Size { get; set; }

        [XmlAttribute("amount")]
        public decimal Amount { get; set; }
    }
}