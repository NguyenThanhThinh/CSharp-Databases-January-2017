namespace MassDefect.App.Export.DTOs
{
    using System.Xml.Serialization;

    [XmlType(TypeName = "victim")]
    public class VictimDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}