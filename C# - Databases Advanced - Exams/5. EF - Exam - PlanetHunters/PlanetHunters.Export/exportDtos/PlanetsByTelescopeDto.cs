namespace PlanetHunters.Export.exportDtos
{
    using System.Collections.Generic;

    public class PlanetsByTelescopeDto
    {
        public string Name { get; set; }

        public float Mass { get; set; }

        public List<string> Orbiting { get; set; }
    }
}