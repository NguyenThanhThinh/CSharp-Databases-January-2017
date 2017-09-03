namespace WeddingsPlanner.Export.DTOs
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

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