namespace MassDefect.App.Import
{
    using Data;
    using Models;
    using System;
    using System.IO;
    using Utilities;
    using Models.DTOs;
    using System.Linq;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class ImportJson
    {
        private const string SolarSystemPath = "../../datasets/solar-systems.json";
        private const string StarsPath = "../../datasets/stars.json";
        private const string PlanetsPath = "../../datasets/planets.json";
        private const string PersonsPath = "../../datasets/persons.json";
        private const string AnomaliesPath = "../../datasets/anomalies.json";
        private const string AnomalyVictimsPath = "../../datasets/anomaly-victims.json";

        public static void SolarSystems()
        {
            using (MassDefectContext context = new MassDefectContext())
            {
                string json = File.ReadAllText(SolarSystemPath);
                IEnumerable<SolarSystemDTO> solarSystemsDtos = JsonConvert.DeserializeObject<IEnumerable<SolarSystemDTO>>(json);

                foreach (SolarSystemDTO solarSystemDto in solarSystemsDtos)
                {
                    if (solarSystemDto.Name == null)
                    {
                        Console.WriteLine(Constants.ImportErrorMessage);
                        continue;
                    }

                    SolarSystem solarSystemEntity = new SolarSystem()
                    {
                        Name = solarSystemDto.Name
                    };

                    context.SolarSystems.Add(solarSystemEntity);
                    Console.WriteLine(Constants.ImportNamedEntitySuccessMessage, "Solar System", solarSystemEntity.Name);
                }

                context.SaveChanges();
            }
        }

        public static void Stars()
        {
            using (MassDefectContext context = new MassDefectContext())
            {
                string json = File.ReadAllText(StarsPath);
                IEnumerable<StarDTO> starsDtos = JsonConvert.DeserializeObject<IEnumerable<StarDTO>>(json);

                foreach (StarDTO starDto in starsDtos)
                {
                    if (starDto.Name == null || starDto.SolarSystem == null)
                    {
                        Console.WriteLine(Constants.ImportErrorMessage);
                        continue;
                    }

                    Star starEntity = new Star()
                    {
                        Name = starDto.Name,
                        SolarSystem = GetSolarSystemByName(starDto.SolarSystem, context)
                    };

                    if (starEntity.SolarSystem == null)
                    {
                        Console.WriteLine(Constants.ImportErrorMessage);
                        continue;
                    }

                    context.Stars.Add(starEntity);
                    Console.WriteLine(Constants.ImportNamedEntitySuccessMessage, "Star", starEntity.Name);
                }

                context.SaveChanges();
            }
        }

        public static void Planets()
        {
            using (MassDefectContext context = new MassDefectContext())
            {
                string json = File.ReadAllText(PlanetsPath);
                IEnumerable<PlanetDTO> planetsDtos = JsonConvert.DeserializeObject<IEnumerable<PlanetDTO>>(json);

                foreach (PlanetDTO planetDto in planetsDtos)
                {
                    if (planetDto.Name == null || planetDto.Sun == null || planetDto.SolarSystem == null)
                    {
                        Console.WriteLine(Constants.ImportErrorMessage);
                        continue;
                    }

                    Planet planetEntity = new Planet()
                    {
                        Name = planetDto.Name,
                        Sun = GetStarByName(planetDto.Sun, context),
                        SolarSystem = GetSolarSystemByName(planetDto.SolarSystem, context)
                    };

                    if (planetEntity.Sun == null || planetEntity.SolarSystem == null)
                    {
                        Console.WriteLine(Constants.ImportErrorMessage);
                        continue;
                    }

                    context.Planets.Add(planetEntity);
                    Console.WriteLine(Constants.ImportNamedEntitySuccessMessage, "Planet", planetEntity.Name);
                }

                context.SaveChanges();
            }
        }

        public static void Persons()
        {
            using (MassDefectContext context = new MassDefectContext())
            {
                string json = File.ReadAllText(PersonsPath);
                IEnumerable<PersonDTO> personsDtos = JsonConvert.DeserializeObject<IEnumerable<PersonDTO>>(json);
                
                foreach (PersonDTO personDto in personsDtos)
                {
                    if (personDto.Name == null || personDto.HomePlanet == null)
                    {
                        Console.WriteLine(Constants.ImportErrorMessage);
                        continue;
                    }

                    Person personEntity = new Person()
                    {
                        Name = personDto.Name,
                        HomePlanet = GetPlanetByName(personDto.HomePlanet, context)
                    };

                    if (personEntity.HomePlanet == null)
                    {
                        Console.WriteLine(Constants.ImportErrorMessage);
                        continue;
                    }

                    context.People.Add(personEntity);
                    Console.WriteLine(Constants.ImportNamedEntitySuccessMessage, "Person", personEntity.Name);
                }

                context.SaveChanges();
            }
        }

        public static void Anomalies()
        {
            using (MassDefectContext context = new MassDefectContext())
            {
                string json = File.ReadAllText(AnomaliesPath);
                IEnumerable<AnomalyDTO> anomaliesDtos = JsonConvert.DeserializeObject<IEnumerable<AnomalyDTO>>(json);

                foreach (AnomalyDTO anomalyDto in anomaliesDtos)
                {
                    if (anomalyDto.OriginPlanet == null || anomalyDto.TeleportPlanet == null)
                    {
                        Console.WriteLine(Constants.ImportErrorMessage);
                        continue;
                    }

                    Anomaly anomalyEntity = new Anomaly()
                    {
                        OriginPlanet = GetPlanetByName(anomalyDto.OriginPlanet, context),
                        TeleportPlanet = GetPlanetByName(anomalyDto.TeleportPlanet, context)
                    };

                    if (anomalyEntity.OriginPlanet == null || anomalyEntity.TeleportPlanet == null)
                    {
                        Console.WriteLine(Constants.ImportErrorMessage);
                        continue;
                    }

                    context.Anomalies.Add(anomalyEntity);
                    Console.WriteLine(Constants.ImportUnnamedEntitySuccessMessage);
                }

                context.SaveChanges();
            }
        }

        public static void AnomalyVictims()
        {
            using (MassDefectContext context = new MassDefectContext())
            {
                string json = File.ReadAllText(AnomalyVictimsPath);
                IEnumerable<AnomalyVictimsDTO> anomalyVictimsDtos = JsonConvert.DeserializeObject<IEnumerable<AnomalyVictimsDTO>>(json);

                foreach (var anomalyVictimDto in anomalyVictimsDtos)
                {
                    if (anomalyVictimDto.Id == null || anomalyVictimDto.Person == null)
                    {
                        Console.WriteLine(Constants.ImportErrorMessage);
                        continue;
                    }

                    Anomaly anomalyEntity = GetAnomalyById(anomalyVictimDto.Id, context);

                    Person personEntity = GetPersonByName(anomalyVictimDto.Person, context);

                    if (anomalyEntity == null || personEntity == null)
                    {
                        Console.WriteLine(Constants.ImportErrorMessage);
                        continue;
                    }

                    anomalyEntity.Victims.Add(personEntity);
                }

                context.SaveChanges();
            }
        }

        private static Anomaly GetAnomalyById(int? anomalyDtoId, MassDefectContext context)
        {
            return context.Anomalies.SingleOrDefault(anomaly => anomaly.Id == anomalyDtoId);
        }

        private static Person GetPersonByName(string personDtoName, MassDefectContext context)
        {
            return context.People.SingleOrDefault(person => person.Name == personDtoName);
        }

        private static Star GetStarByName(string planetDtoStarName, MassDefectContext context)
        {
            return context.Stars.SingleOrDefault(star => star.Name == planetDtoStarName);
        }

        private static SolarSystem GetSolarSystemByName(string solarSystemDtoName, MassDefectContext context)
        {
            return context.SolarSystems.SingleOrDefault(solarSystem => solarSystem.Name == solarSystemDtoName);
        }

        private static Planet GetPlanetByName(string personDtoPlanetName, MassDefectContext context)
        {
            return context.Planets.SingleOrDefault(planet => planet.Name == personDtoPlanetName);
        }
    }
}
