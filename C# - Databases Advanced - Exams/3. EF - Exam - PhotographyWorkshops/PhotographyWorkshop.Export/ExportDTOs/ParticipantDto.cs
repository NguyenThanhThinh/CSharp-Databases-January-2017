using System.Collections.Generic;
using System.Xml.Serialization;

namespace PhotographyWorkshop.Export.ExportDTOs
{
    [XmlType(TypeName = "participants")]
    public class ParticipantDto
    {
        [XmlAttribute("count")]
        public int ParticipantCount { get; set; }

        [XmlElement("participant")]
        public List<string> Names { get; set; }
    }
}
