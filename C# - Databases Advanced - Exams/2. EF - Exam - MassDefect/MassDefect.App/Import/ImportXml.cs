namespace MassDefect.App.Import
{
    using Data;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using Utilities;

    internal class ImportXml
    {
        private const string NewAnomaliesPath = "../../datasets/new-anomalies.xml";

        public static void NewAnomalies()
        {
            XDocument documentXml = XDocument.Load(NewAnomaliesPath);

            //IEnumerable<XElement> anomalies = documentXml.Root?.Elements();
            IEnumerable<XElement> anomalies = documentXml.XPathSelectElements("anomalies/anomaly");

            using (MassDefectContext context = new MassDefectContext())
            {
                foreach (XElement anomaly in anomalies)
                {
                    ImportAnomalyAndVictims(anomaly, context);
                }
            }
        }

        private static void ImportAnomalyAndVictims(XElement anomalyXml, MassDefectContext context)
        {
            string originPlanetName = anomalyXml.Attribute("origin-planet")?.Value;
            string teleportPlanetName = anomalyXml.Attribute("teleport-planet")?.Value;

            if (originPlanetName == null || teleportPlanetName == null)
            {
                Console.WriteLine(Constants.ImportErrorMessage);
                return;
            }

            Anomaly anomalyEntity = new Anomaly()
            {
                OriginPlanet = GetPlanetByName(originPlanetName, context),
                TeleportPlanet = GetPlanetByName(teleportPlanetName, context)
            };

            if (anomalyEntity.OriginPlanet == null || anomalyEntity.TeleportPlanet == null)
            {
                Console.WriteLine(Constants.ImportErrorMessage);
                return;
            }

            context.Anomalies.Add(anomalyEntity);
            Console.WriteLine(Constants.ImportUnnamedEntitySuccessMessage);

            IEnumerable<XElement> victims = anomalyXml.XPathSelectElements("victims/victim");

            foreach (XElement victim in victims)
            {
                ImportVictim(victim, context, anomalyEntity);
            }

            context.SaveChanges();
        }

        private static Planet GetPlanetByName(string planetName, MassDefectContext context)
        {
            return context.Planets.SingleOrDefault(planet => planet.Name == planetName);
        }

        private static Person GetPersonByName(string victimName, MassDefectContext context)
        {
            return context.People.SingleOrDefault(person => person.Name == victimName);
        }

        private static void ImportVictim(XElement victim, MassDefectContext context, Anomaly anomalyEntity)
        {
            string victimName = victim.Attribute("name")?.Value;

            if (victimName == null)
            {
                Console.WriteLine(Constants.ImportErrorMessage);
                return;
            }

            Person personEntity = GetPersonByName(victimName, context);

            if (personEntity == null)
            {
                Console.WriteLine(Constants.ImportErrorMessage);
                return;
            }

            anomalyEntity.Victims.Add(personEntity);
        }
    }
}