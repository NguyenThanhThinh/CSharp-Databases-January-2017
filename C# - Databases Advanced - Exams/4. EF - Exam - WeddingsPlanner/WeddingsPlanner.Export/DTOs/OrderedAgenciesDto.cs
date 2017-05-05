namespace WeddingsPlanner.Export.DTOs
{
    using Newtonsoft.Json;

    public class OrderedAgenciesDto
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "count")]
        public int? Count { get; set; }

        [JsonProperty(PropertyName = "town")]
        public string Town { get; set; }
    }
}
