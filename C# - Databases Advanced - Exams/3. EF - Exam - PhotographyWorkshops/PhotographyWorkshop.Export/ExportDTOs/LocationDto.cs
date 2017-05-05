using System.Collections.Generic;
using System.Xml.Serialization;

namespace PhotographyWorkshop.Export.ExportDTOs
{
    [XmlType("location")]
    public class LocationDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("workshop")]
        public List<WorkshopDto> WorkshopsDtos { get; set; }
    }
}
