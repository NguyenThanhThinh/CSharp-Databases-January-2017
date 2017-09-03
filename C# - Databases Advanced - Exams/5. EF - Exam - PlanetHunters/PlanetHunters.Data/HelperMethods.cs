namespace PlanetHunters.Data
{
    using Models;
    using System;
    using System.Data.Entity.Validation;
    using System.Linq;
    using utilities;

    public class HelperMethods
    {
        public static void AddAstronomerToDatabase(PlanetHuntersContext context, Astronomer astronomerEntity)
        {
            try
            {
                context.Astronomers.Add(astronomerEntity);
                context.SaveChanges();
                Console.WriteLine(Messages.AstronomerImported, astronomerEntity.FirstName, astronomerEntity.LastName);
            }
            catch (DbEntityValidationException)
            {
                context.Astronomers.Remove(astronomerEntity);
                Console.WriteLine(Messages.Error);
            }
        }

        public static void AddTelescopeToDatabase(PlanetHuntersContext context, Telescope telescopeEntity)
        {
            try
            {
                context.Telescopes.Add(telescopeEntity);
                context.SaveChanges();
                Console.WriteLine(Messages.TelescopeImported, telescopeEntity.Name);
            }
            catch (DbEntityValidationException)
            {
                context.Telescopes.Remove(telescopeEntity);
                Console.WriteLine(Messages.Error);
            }
        }

        public static bool IsStarSystemExisting(PlanetHuntersContext context, string starSystemName)
        {
            StarSystem starSystem = context.StarSystems.FirstOrDefault(system => system.Name == starSystemName);
            if (starSystem == null)
            {
                return false;
            }

            return true;
        }

        public static StarSystem GetStarSystem(PlanetHuntersContext context, string starSystemName)
        {
            return context.StarSystems.FirstOrDefault(system => system.Name == starSystemName);
        }

        public static void AddStarSystemToDatabase(PlanetHuntersContext context, StarSystem starSystem)
        {
            try
            {
                context.StarSystems.Add(starSystem);
                context.SaveChanges();
                Console.WriteLine(Messages.StarSystemImported, starSystem.Name);
            }
            catch (DbEntityValidationException)
            {
                context.StarSystems.Remove(starSystem);
                Console.WriteLine(Messages.Error);
            }
        }

        public static void AddPlanetToDatabase(PlanetHuntersContext context, Planet planetEntity)
        {
            try
            {
                context.Planets.Add(planetEntity);
                context.SaveChanges();
                Console.WriteLine(Messages.PlanetImported, planetEntity.Name);
            }
            catch (DbEntityValidationException)
            {
                context.Planets.Remove(planetEntity);
                Console.WriteLine(Messages.Error);
            }
        }

        public static void AddStarToDatabase(PlanetHuntersContext context, Star starEntity)
        {
            try
            {
                context.Stars.Add(starEntity);
                context.SaveChanges();
                Console.WriteLine(Messages.StarImported, starEntity.Name);
            }
            catch (DbEntityValidationException)
            {
                context.Stars.Remove(starEntity);
                Console.WriteLine(Messages.Error);
            }
        }

        public static bool IsTelescopeExisting(PlanetHuntersContext context, string telescopeNameAsString)
        {
            Telescope telescopeEntity = context.Telescopes.FirstOrDefault(telescope => telescope.Name == telescopeNameAsString);
            if (telescopeEntity == null)
            {
                return false;
            }

            return true;
        }

        public static Telescope GetTelescope(PlanetHuntersContext context, string tescopeName)
        {
            return context.Telescopes.FirstOrDefault(telescope => telescope.Name == tescopeName);
        }

        public static bool IsStarExisting(PlanetHuntersContext context, string starName)
        {
            Star starEntity = context.Stars.FirstOrDefault(star => star.Name == starName);
            if (starEntity == null)
            {
                return false;
            }

            return true;
        }

        public static Star GetStar(PlanetHuntersContext context, string starName)
        {
            return context.Stars.FirstOrDefault(star => star.Name == starName);
        }

        public static bool IsPlanetExisting(PlanetHuntersContext context, string planetName)
        {
            Planet planetEntity = context.Planets.FirstOrDefault(planet => planet.Name == planetName);
            if (planetEntity == null)
            {
                return false;
            }

            return true;
        }

        public static Planet GetPlanet(PlanetHuntersContext context, string planetName)
        {
            return context.Planets.FirstOrDefault(planet => planet.Name == planetName);
        }

        public static bool IsAstronomerExisting(PlanetHuntersContext context, string firstName, string lastName)
        {
            Astronomer astronomerEntity = context.Astronomers
                .FirstOrDefault(astronomer => astronomer.FirstName == firstName && astronomer.LastName == lastName);
            if (astronomerEntity == null)
            {
                return false;
            }

            return true;
        }

        public static Astronomer GetAstronomer(PlanetHuntersContext context, string firstName, string lastName)
        {
            return context.Astronomers
                .FirstOrDefault(astronomer => astronomer.FirstName == firstName && astronomer.LastName == lastName);
        }

        public static void AddDiscoveryToDatabase(PlanetHuntersContext context, Discovery discoveryEntity)
        {
            try
            {
                context.Discoveries.Add(discoveryEntity);
                context.SaveChanges();
                Console.WriteLine(Messages.DiscoveryImported, discoveryEntity.DateMade, discoveryEntity.TelesopeUsed.Name,
                    discoveryEntity.Stars.Count, discoveryEntity.Planets.Count, discoveryEntity.Pioneers.Count, discoveryEntity.Observers.Count);
            }
            catch (DbEntityValidationException)
            {
                context.Discoveries.Remove(discoveryEntity);
                Console.WriteLine(Messages.Error);
            }
        }
    }
}