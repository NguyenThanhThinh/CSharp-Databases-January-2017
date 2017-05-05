using Newtonsoft.Json;

namespace MassDefect.App.Export.DTOs
{
    public class PlanetDto
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
