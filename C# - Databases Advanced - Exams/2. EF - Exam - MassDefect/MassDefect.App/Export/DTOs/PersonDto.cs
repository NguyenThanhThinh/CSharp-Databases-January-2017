using Newtonsoft.Json;

namespace MassDefect.App.Export.DTOs
{
    public class PersonDto
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "homePlanet")]
        public PlanetDto HomePlanet { get; set; }
    }
}