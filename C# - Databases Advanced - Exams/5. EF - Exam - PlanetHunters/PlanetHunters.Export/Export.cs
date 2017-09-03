namespace PlanetHunters.Export
{
    using Data;
    using exportDtos;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;
    using utilities;

    internal class Export
    {
        private static void Main(string[] args)
        {
            //ExporPlanetsByTelescope("TRAPPIST");
            //ExportAstronomers("Alpha Centauri");

            //ExportStarsWithSerializer();
        }

        private static void ExportJsonToFolder<Entity>(string pathToExport, Entity entityType)
        {
            string json = JsonConvert.SerializeObject(entityType, Formatting.Indented);
            File.WriteAllText(pathToExport, json);
        }

        private static void ExportXmlToFolder<Entity>(Entity entityType, string pathToExport, string rootAttributeValue)
        {
            XmlSerializer serializer = new XmlSerializer(entityType.GetType(), new XmlRootAttribute(rootAttributeValue));

            StreamWriter writer = new StreamWriter(pathToExport);

            using (writer)
            {
                serializer.Serialize(writer, entityType);
            }
        }

        private static void ExporPlanetsByTelescope(string telescopeName)
        {
            using (PlanetHuntersContext context = new PlanetHuntersContext())
            {
                List<PlanetsByTelescopeDto> planetsFiltered = context.Planets
                    .Where(planet => planet.Discovery.TelesopeUsed.Name == telescopeName)
                    .Select(planet => new PlanetsByTelescopeDto()
                    {
                        Name = planet.Name,
                        Mass = planet.Mass,
                        Orbiting = planet.HostStarSystem.Stars.Select(star => star.Name).ToList()
                    })
                    .OrderByDescending(planet => planet.Mass)
                    .ToList();

                ExportJsonToFolder(string.Format(Constants.planetsByTelescopePath, telescopeName), planetsFiltered);
            }
        }

        private static void ExportAstronomers(string starSystemName)
        {
            using (PlanetHuntersContext context = new PlanetHuntersContext())
            {
                List<AstronomerDto> astronomersFiltered = context.Astronomers
                    .Where(astronomer => astronomer.DiscoveriesMade
                                             .Any(
                                                 discovery =>
                                                     discovery.Planets.Any(
                                                         planet => planet.HostStarSystem.Name == starSystemName) ||
                                                     discovery.Stars.Any(
                                                         star => star.HostStarSystem.Name == starSystemName)) ||
                                         astronomer.DiscoveriesObserved
                                             .Any(
                                                 discovery =>
                                                     discovery.Planets.Any(
                                                         planet => planet.HostStarSystem.Name == starSystemName) ||
                                                     discovery.Stars.Any(
                                                         star => star.HostStarSystem.Name == starSystemName)))
                    .OrderBy(astronomer => astronomer.LastName)
                    .Select(astronomer => new AstronomerDto()
                    {
                        Name = astronomer.FirstName + " " + astronomer.LastName,
                        Role = astronomer.DiscoveriesMade.Any(
                                        discovery => discovery.Pioneers.Any(
                                             pioneer => pioneer.FirstName == astronomer.FirstName && pioneer.LastName == astronomer.LastName))
                                             ? "pioneer" : "observer"
                    })
                    .ToList();

                ExportJsonToFolder(string.Format(Constants.astronomersPath, starSystemName), astronomersFiltered);
            }
        }

        private static void ExportStarsWithSerializer()
        {
            using (PlanetHuntersContext context = new PlanetHuntersContext())
            {
                List<StarDto> starsFiltered = context.Stars
                    .Where(star => star.DiscoveryId != null)
                    .Select(star => new StarDto()
                    {
                        Name = star.Name,
                        Temperature = star.Temperature,
                        StarSystem = star.HostStarSystem.Name,
                        DiscoveryInfo = new DiscoveryInfoDto()
                        {
                            DiscoveryDate = star.Discovery.DateMade,
                            TelescopeName = star.Discovery.TelesopeUsed.Name,
                            AsronomerPionersDto = star.Discovery.Pioneers
                                .Select(pioner => new AstronomerPioneerDto()
                                {
                                    Name = pioner.FirstName + " " + pioner.LastName,
                                    Pioneer = true
                                })
                                .Union(star.Discovery.Observers.Select(obsever => new AstronomerPioneerDto()
                                {
                                    Name = obsever.FirstName + " " + obsever.LastName,
                                    Pioneer = false
                                })
                                .ToList())
                            .ToList(),
                        }
                    }).ToList();

                ExportXmlToFolder(starsFiltered, Constants.starsPath, "Stars");
            }
        }
    }
}