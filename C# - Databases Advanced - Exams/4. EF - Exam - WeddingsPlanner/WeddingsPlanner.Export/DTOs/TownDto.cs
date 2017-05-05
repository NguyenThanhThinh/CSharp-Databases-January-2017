namespace WeddingsPlanner.Export.DTOs
{
    using System.Xml.Serialization;
    using System.Collections.Generic;

    [XmlType("town")]
    public class TownDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlArray("agencies")]
        [XmlArrayItem("agency")]
        public List<AgencyInTownDto> AgenciesDtos { get; set; }
    }
}
