namespace MassDefect.App.Export
{
    using Data;
    using DTOs;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class ExportJson
    {
        public static void ExportPlanetsWhichAreNotAnomalyOrigins()
        {
            using (MassDefectContext context = new MassDefectContext())
            {
                List<PlanetDto> planets = context.Planets
                    .Where(planet => !planet.OriginAnomalies.Any())
                    .Select(planet => new PlanetDto()
                    {
                        Name = planet.Name
                    })
                .ToList();

                string json = JsonConvert.SerializeObject(planets, Formatting.Indented);
                File.WriteAllText("../../planets.json", json);
            }
        }

        public static void ExportPeopleWhichHaveNotBeenVictims()
        {
            using (MassDefectContext context = new MassDefectContext())
            {
                List<PersonDto> people = context.People
                    .Where(person => !person.Anomalies.Any())
                    .Select(person => new PersonDto
                    {
                        Name = person.Name,
                        HomePlanet = new PlanetDto
                        {
                            Name = person.HomePlanet.Name
                        }
                    })
                .ToList();

                string json = JsonConvert.SerializeObject(people, Formatting.Indented);
                File.WriteAllText("../../people.json", json);
            }
        }

        public static void ExportTopAnomaly()
        {
            using (MassDefectContext context = new MassDefectContext())
            {
                var anomalyMax = context.Anomalies
                    .OrderByDescending(anomaly => anomaly.Victims.Count)
                    .Take(1)
                    .Select(anomaly => new AnomalyDto
                    {
                        Id = anomaly.Id,
                        OriginPlanet = new PlanetDto()
                        {
                            Name = anomaly.OriginPlanet.Name
                        },
                        TeleportPlanet = new PlanetDto()
                        {
                            Name = anomaly.TeleportPlanet.Name
                        },
                        VictimsCount = anomaly.Victims.Count
                    });

                string json = JsonConvert.SerializeObject(anomalyMax, Formatting.Indented);
                File.WriteAllText("../../anomaly.json", json);
            }
        }
    }
}