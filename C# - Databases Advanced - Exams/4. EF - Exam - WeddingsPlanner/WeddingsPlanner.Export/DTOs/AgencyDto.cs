namespace WeddingsPlanner.Export.DTOs
{
    using Newtonsoft.Json;

    public class AgencyDto
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "town")]
        public string Town { get; set; }
    }
}