namespace PhotographyWorkshop.Export.ExportDTOs
{
    using System.Xml.Serialization;
    using System.Collections.Generic;

    [XmlType(TypeName = "photographer")]
    public class SameCameMakePhotographersDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("primary-camera")]
        public string PrimaryCamera { get; set; }

        [XmlArray(ElementName = "lenses")]
        [XmlArrayItem(ElementName = "lens")]
        public List<string> Lenses { get; set; }

        public bool ShouldSerializeLenses()
        {
            return Lenses.Count != 0;
        }
    }
}
