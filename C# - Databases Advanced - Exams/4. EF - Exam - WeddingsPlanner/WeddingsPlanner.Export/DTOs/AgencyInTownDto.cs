using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WeddingsPlanner.Export.DTOs
{
    [XmlType("agency")]
    public class AgencyInTownDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("profit")]
        public decimal Profit { get; set; }

        [XmlElement("wedding")]
        public List<WeddingDto> Weddings { get; set; }
    }
}
