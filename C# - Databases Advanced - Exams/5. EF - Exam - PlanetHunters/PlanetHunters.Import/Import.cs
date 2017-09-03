namespace PlanetHunters.Import
{
    using Data;
    using Data.utilities;
    using importDtos;
    using Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using utilities;

    internal class Import
    {
        private static void Main(string[] args)
        {
            ImportAstronomers();
            ImportTelescopes();
            ImportPlanets();
            ImportStars();
            ImportDiscoveries();
        }

        private static List<Entity> ParseJson<Entity>(string filePath)
        {
            string json = File.ReadAllText(filePath);
            List<Entity> dtos = JsonConvert.DeserializeObject<List<Entity>>(json);

            return dtos;
        }

        private static void ImportAstronomers()
        {
            using (PlanetHuntersContext context = new PlanetHuntersContext())
            {
                List<AstronomerDto> astronomerDtos = ParseJson<AstronomerDto>(Constants.AstronomersPath);

                foreach (AstronomerDto astronomerDto in astronomerDtos)
                {
                    // We can make the checks for FirstName and LastName like this here, then
                    // add the entity to the Database and save it with context.SaveChanges() with no errors
                    // (but with this design we are skipping the attribute validations in the models)

                    // OR

                    // In order to use all the attribute validations which we have implemented in the models,
                    // we can skip the validations here and just use a try-catch construction with Context.SaveChanges
                    // so can the attributes to work as expected (we have to add the entity to Database in 'try', then remove it in 'catch');

                    if (astronomerDto.FirstName == null || astronomerDto.FirstName.Length > 50)
                    {
                        Console.WriteLine(Messages.Error);
                        continue;
                    }

                    if (astronomerDto.LastName == null || astronomerDto.LastName.Length > 50)
                    {
                        Console.WriteLine(Messages.Error);
                        continue;
                    }

                    Astronomer astronomerEntity = new Astronomer()
                    {
                        FirstName = astronomerDto.FirstName,
                        LastName = astronomerDto.LastName
                    };

                    HelperMethods.AddAstronomerToDatabase(context, astronomerEntity);
                }
            }
        }

        private static void ImportTelescopes()
        {
            using (PlanetHuntersContext context = new PlanetHuntersContext())
            {
                List<TelescopeDto> telescopeDtos = ParseJson<TelescopeDto>(Constants.TelescopesPath);

                foreach (TelescopeDto telescopeDto in telescopeDtos)
                {
                    if (telescopeDto.Name == null || telescopeDto.Name.Length > 255)
                    {
                        Console.WriteLine(Messages.Error);
                        continue;
                    }

                    if (telescopeDto.Location == null || telescopeDto.Location.Length > 255)
                    {
                        Console.WriteLine(Messages.Error);
                        continue;
                    }

                    if (telescopeDto.MirrorDiameter < 0.0001f)
                    {
                        Console.WriteLine(Messages.Error);
                        continue;
                    }

                    Telescope telescopeEntity = new Telescope()
                    {
                        Name = telescopeDto.Name,
                        Location = telescopeDto.Location,
                        MirrorDiameter = telescopeDto.MirrorDiameter
                    };

                    HelperMethods.AddTelescopeToDatabase(context, telescopeEntity);
                }
            }
        }

        private static void ImportPlanets()
        {
            using (PlanetHuntersContext context = new PlanetHuntersContext())
            {
                List<PlanetDto> planetDtos = ParseJson<PlanetDto>(Constants.PlanetsPath);

                foreach (PlanetDto planetDto in planetDtos)
                {
                    if (planetDto.Name == null || planetDto.Name.Length > 255)
                    {
                        Console.WriteLine(Messages.Error);
                        continue;
                    }

                    if (planetDto.Mass < 0.0001f)
                    {
                        Console.WriteLine(Messages.Error);
                        continue;
                    }

                    Planet planetEntity = new Planet()
                    {
                        Name = planetDto.Name,
                        Mass = planetDto.Mass
                    };

                    if (HelperMethods.IsStarSystemExisting(context, planetDto.StarSystem))
                    {
                        planetEntity.HostStarSystem = HelperMethods.GetStarSystem(context, planetDto.StarSystem);
                    }
                    else
                    {
                        if (planetDto.StarSystem == null || planetDto.StarSystem.Length > 255)
                        {
                            Console.WriteLine(Messages.Error);
                            continue;
                        }

                        StarSystem starSystemEntity = new StarSystem()
                        {
                            Name = planetDto.StarSystem
                        };

                        HelperMethods.AddStarSystemToDatabase(context, starSystemEntity);
                        planetEntity.HostStarSystem = starSystemEntity;
                    }

                    HelperMethods.AddPlanetToDatabase(context, planetEntity);
                }
            }
        }

        private static void ImportStars()
        {
            XDocument documentXml = XDocument.Load(Constants.StarsPath);

            IEnumerable<XElement> starsNode = documentXml.XPathSelectElements("Stars/Star");

            using (PlanetHuntersContext context = new PlanetHuntersContext())
            {
                foreach (XElement starNode in starsNode)
                {
                    string starName = starNode.Element("Name")?.Value;
                    string starTemperatureAsString = starNode.Element("Temperature")?.Value;
                    string starSystemName = starNode.Element("StarSystem")?.Value;

                    if (starName == null || starName.Length > 255)
                    {
                        Console.WriteLine(Messages.Error);
                        continue;
                    }

                    if (starTemperatureAsString == null)
                    {
                        Console.WriteLine(Messages.Error);
                        continue;
                    }

                    int starTemperature = int.Parse(starTemperatureAsString);
                    if (starTemperature < 2400)
                    {
                        Console.WriteLine(Messages.Error);
                        continue;
                    }

                    Star starEntity = new Star()
                    {
                        Name = starName,
                        Temperature = starTemperature
                    };

                    if (HelperMethods.IsStarSystemExisting(context, starSystemName))
                    {
                        starEntity.HostStarSystem = HelperMethods.GetStarSystem(context, starSystemName);
                    }
                    else
                    {
                        if (starSystemName == null || starSystemName.Length > 255)
                        {
                            Console.WriteLine(Messages.Error);
                            continue;
                        }

                        StarSystem starSystemEntity = new StarSystem()
                        {
                            Name = starSystemName
                        };

                        HelperMethods.AddStarSystemToDatabase(context, starSystemEntity);
                        starEntity.HostStarSystem = starSystemEntity;
                    }

                    HelperMethods.AddStarToDatabase(context, starEntity);
                }
            }
        }

        private static void ImportDiscoveries()
        {
            XDocument documentXml = XDocument.Load(Constants.DiscoveriesPath);

            IEnumerable<XElement> discoveriesNode = documentXml.XPathSelectElements("Discoveries/Discovery");

            using (PlanetHuntersContext context = new PlanetHuntersContext())
            {
                foreach (XElement discoveryNode in discoveriesNode)
                {
                    string dateMadeAsString = discoveryNode.Attribute("DateMade")?.Value;
                    string telescopeNameAsString = discoveryNode.Attribute("Telescope")?.Value;
                    bool ignoreDiscovery = false;

                    if (dateMadeAsString == null)
                    {
                        Console.WriteLine(Messages.Error);
                        continue;
                    }

                    DateTime dateMade = DateTime.Parse(dateMadeAsString);

                    if (telescopeNameAsString == null)
                    {
                        Console.WriteLine(Messages.Error);
                        continue;
                    }

                    if (!HelperMethods.IsTelescopeExisting(context, telescopeNameAsString))
                    {
                        Console.WriteLine(Messages.Error);
                        continue;
                    }

                    Telescope telescopeEntity = HelperMethods.GetTelescope(context, telescopeNameAsString);

                    Discovery discoveryEntity = new Discovery()
                    {
                        DateMade = dateMade,
                        TelesopeUsed = telescopeEntity
                    };

                    IEnumerable<XElement> starsNode = discoveryNode.XPathSelectElements("Stars/Star");

                    foreach (XElement starNode in starsNode)
                    {
                        string starName = starNode.Value;

                        if (!HelperMethods.IsStarExisting(context, starName))
                        {
                            ignoreDiscovery = true;
                            break;
                        }

                        discoveryEntity.Stars.Add(HelperMethods.GetStar(context, starName));
                    }

                    if (ignoreDiscovery)
                    {
                        continue;
                    }

                    IEnumerable<XElement> planetsNode = discoveryNode.XPathSelectElements("Planets/Planet");

                    foreach (XElement planetNode in planetsNode)
                    {
                        string planetName = planetNode.Value;

                        if (!HelperMethods.IsPlanetExisting(context, planetName))
                        {
                            ignoreDiscovery = true;
                            break;
                        }

                        discoveryEntity.Planets.Add(HelperMethods.GetPlanet(context, planetName));
                    }

                    if (ignoreDiscovery)
                    {
                        continue;
                    }

                    IEnumerable<XElement> pioneersNode = discoveryNode.XPathSelectElements("Pioneers/Astronomer");

                    foreach (XElement pioneerNode in pioneersNode)
                    {
                        string pioneerName = pioneerNode.Value;
                        string[] pionerNameArgs = pioneerName.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        string firstName = pionerNameArgs[1];
                        string lastName = pionerNameArgs[0];

                        if (!HelperMethods.IsAstronomerExisting(context, firstName, lastName))
                        {
                            ignoreDiscovery = true;
                            break;
                        }

                        discoveryEntity.Pioneers.Add(HelperMethods.GetAstronomer(context, firstName, lastName));
                    }

                    if (ignoreDiscovery)
                    {
                        continue;
                    }

                    IEnumerable<XElement> observersNode = discoveryNode.XPathSelectElements("Observers/Astronomer");

                    foreach (XElement observerNode in observersNode)
                    {
                        string observerName = observerNode.Value;
                        string[] observerNameArgs = observerName.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        string firstName = observerNameArgs[1];
                        string lastName = observerNameArgs[0];

                        if (!HelperMethods.IsAstronomerExisting(context, firstName, lastName))
                        {
                            ignoreDiscovery = true;
                            break;
                        }

                        discoveryEntity.Observers.Add(HelperMethods.GetAstronomer(context, firstName, lastName));
                    }

                    if (ignoreDiscovery)
                    {
                        continue;
                    }

                    HelperMethods.AddDiscoveryToDatabase(context, discoveryEntity);
                }
            }
        }
    }
}