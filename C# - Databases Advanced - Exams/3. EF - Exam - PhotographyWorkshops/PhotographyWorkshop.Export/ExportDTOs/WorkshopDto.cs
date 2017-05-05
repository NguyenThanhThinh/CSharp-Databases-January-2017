using System.Collections.Generic;
using System.Xml.Serialization;

namespace PhotographyWorkshop.Export.ExportDTOs
{
    [XmlType(TypeName = "workshop")]
    public class WorkshopDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("total-profit")]
        public decimal TotalProfit { get; set; }

        [XmlElement("participants")]
        public ParticipantDto ParticipantDto { get; set; }
    }
}
