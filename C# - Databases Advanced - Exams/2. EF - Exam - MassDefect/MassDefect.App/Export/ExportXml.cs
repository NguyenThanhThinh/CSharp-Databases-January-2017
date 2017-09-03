namespace MassDefect.App.Export
{
    using Data;
    using DTOs;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    public class ExportXml
    {
        public static void ExtractAllAnomaliesAndPeopleAffected()
        {
            using (MassDefectContext context = new MassDefectContext())
            {
                XDocument documentXml = new XDocument();

                List<AnomalyWithVictimsDto> anomaliesDtos = context.Anomalies
                    .Select(anomaly => new AnomalyWithVictimsDto
                    {
                        Id = anomaly.Id,
                        OriginPlanet = anomaly.OriginPlanet.Name,
                        TeleportPlanet = anomaly.TeleportPlanet.Name,
                        Victims = anomaly.Victims.Select(victim => new VictimDto()
                        {
                            Name = victim.Name
                        }).ToList()
                    })
                    .OrderBy(anomaly => anomaly.Id)
                    .ToList();

                //// With 'Serializer'

                var serializer = new XmlSerializer(anomaliesDtos.GetType(), new XmlRootAttribute("anomalies"));

                var writer = new StreamWriter("../../anomaliesAndAffectedPeople.xml");

                using (writer)
                {
                    serializer.Serialize(writer, anomaliesDtos);
                }

                //// Without 'Serializer'

                //XElement anomaliesXml = new XElement("anomalies");

                //foreach (var anomalyDto in anomaliesDtos)
                //{
                //    XElement anomalyXml = new XElement("anomaly");
                //    anomalyXml.SetAttributeValue("id", anomalyDto.Id);
                //    anomalyXml.SetAttributeValue("origin-planet", anomalyDto.OriginPlanet);
                //    anomalyXml.SetAttributeValue("teleport-planet", anomalyDto.TeleportPlanet);

                //    XElement anomalyVictimsXml = new XElement("victims");

                //    foreach (var victim in anomalyDto.Victims)
                //    {
                //        XElement anomalyVictimXml = new XElement("victim");
                //        anomalyVictimXml.SetAttributeValue("name", victim.Name);

                //        anomalyVictimsXml.Add(anomalyVictimXml);
                //    }

                //    anomalyXml.Add(anomalyVictimsXml);
                //    anomaliesXml.Add(anomalyXml);
                //}

                //documentXml.Add(anomaliesXml);
                //documentXml.Save("../../anomaliesAndAffectedPeople.xml");
            }
        }
    }
}