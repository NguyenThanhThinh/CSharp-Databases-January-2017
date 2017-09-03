using Newtonsoft.Json;

namespace MassDefect.App.Export.DTOs
{
    public class AnomalyDto
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "originPlanet")]
        public PlanetDto OriginPlanet { get; set; }

        [JsonProperty(PropertyName = "teleportPlanet")]
        public PlanetDto TeleportPlanet { get; set; }

        [JsonProperty(PropertyName = "victimsCount")]
        public int VictimsCount { get; set; }
    }
}