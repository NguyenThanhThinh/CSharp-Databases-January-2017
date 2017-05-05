namespace PhotographyWorkshop.Export
{
    using Data;
    using Utilities;
    using System.IO;
    using ExportDTOs;
    using System.Linq;
    using System.Xml.Linq;
    using PhotographyWorkshops.Models;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public class Xml
    {
        private static void ExportXmlToFolder<TEntity>(TEntity entityType, string pathToExport, string rootAttributeValue)
        {
            XmlSerializer serializer = new XmlSerializer(entityType.GetType(), new XmlRootAttribute(rootAttributeValue));

            StreamWriter writer = new StreamWriter(pathToExport);

            using (writer)
            {
                serializer.Serialize(writer, entityType);
            }
        }

        public static void ExportPhotographersWithSameCameraMake()
        {
            using (PhotographyWorkshopContext context = new PhotographyWorkshopContext())
            {
                //// With 'Serialize'

                List<SameCameMakePhotographersDto> photographers = context.Photographers
                    .Where(photographer => photographer.PrimaryCamera.Make == photographer.SecondaryCamera.Make)
                    .Select(photographer => new SameCameMakePhotographersDto
                    {
                        Name = photographer.FirstName + " " + photographer.LastName,
                        PrimaryCamera = photographer.PrimaryCamera.Make + " " + photographer.PrimaryCamera.Model,
                        Lenses = photographer.Lenses
                                                .Select(lens => lens.Make + " " + lens.FocalLength + "mm f" + lens.MaxAperture)
                                                .ToList()
                    })
                    .ToList();

                ExportXmlToFolder(photographers, Constants.ExportPhotographersWithSameCameraMake, "photographers");

                //// Without 'Serializer'

                //var photographers = context.Photographers
                //    .Where(photographer => photographer.PrimaryCamera.Make == photographer.SecondaryCamera.Make)
                //    .Select(photographer => new
                //    {
                //        Name = photographer.FirstName + " " + photographer.LastName,
                //        PrimaryCamera = photographer.PrimaryCamera.Make + " " + photographer.PrimaryCamera.Model,
                //        Lenses = photographer.Lenses.Select(lens => new
                //        {
                //            lens.Make,
                //            lens.FocalLength,
                //            lens.MaxAperture
                //        })                       
                //    })
                //    .ToList();

                //XDocument documentXml = new XDocument();

                //XElement photographersListXml = new XElement("photographers");

                //foreach (var photographer in photographers)
                //{
                //    XElement photographerXml = new XElement("photographer");
                //    photographerXml.SetAttributeValue("name", photographer.Name);
                //    photographerXml.SetAttributeValue("primary-camera", photographer.PrimaryCamera);

                //    if (photographer.Lenses.Any())
                //    {
                //        XElement lensesListXml = new XElement("lenses");
                //        foreach (var lens in photographer.Lenses)
                //        {
                //            XElement lensXml = new XElement("lens");
                //            lensXml.Value = lens.Make + " " + lens.FocalLength + " " + lens.MaxAperture;

                //            lensesListXml.Add(lensXml);
                //        }

                //        photographerXml.Add(lensesListXml);
                //    }

                //    photographersListXml.Add(photographerXml);
                //}

                //documentXml.Add(photographersListXml);
                //documentXml.Save("../../../datasets/same-cameras-photographers.xml");
            }
        }

        public static void ExportWorkshopsByLocation()
        {
            using (PhotographyWorkshopContext context = new PhotographyWorkshopContext())
            {
                //// With 'Serializer'

                List<LocationDto> workshopsByLocation = context.Workshops
                    .Where(workshop => workshop.Participants.Count >= 5)
                    .GroupBy(workshop => workshop.Location, workshop => workshop, (location, workshops) => new
                    {
                        Location = location,
                        Workshops = workshops
                    })
                    .Where(ws => ws.Workshops.Any())
                    .Select(wsByLocation => new LocationDto
                    {
                        Name = wsByLocation.Location,
                        WorkshopsDtos = wsByLocation.Workshops.Select(ws => new WorkshopDto
                        {
                            Name = ws.Name,
                            //TotalProfit = CalculateTotalProfit(ws) - methods are't working in LINQ
                            TotalProfit = (ws.Participants.Count * ws.PricePerParticipant) -
                                          ((ws.Participants.Count * ws.PricePerParticipant) * 0.2m),
                            ParticipantDto = new ParticipantDto
                            {
                                ParticipantCount = ws.Participants.Count,
                                Names = ws.Participants.Select(part => part.FirstName + " " + part.LastName).ToList()
                            }
                        }).ToList()
                    })
                    .ToList();

                ExportXmlToFolder(workshopsByLocation, Constants.ExportWorkshopsByLocation, "locations");

                //// Without 'Serializer'

                // Without the Select clause the result is the same
                //var workshopsByLocation = context.Workshops
                //    .Where(workshop => workshop.Participants.Count >= 5)
                //    .GroupBy(workshop => workshop.Location, workshop => workshop, (location, workshops) => new
                //    {
                //        Location = location,
                //        Workshops = workshops
                //    })
                //    .Where(ws => ws.Workshops.Any())
                //    .Select(wsByLocation => new
                //    {
                //        wsByLocation.Location,
                //        wsByLocation.Workshops,
                //    });

                // XDocument documentXml = new XDocument();

                // XElement locationsListXml = new XElement("locations");

                // foreach (var location in workshopsByLocation)
                // {
                //     XElement locationXml = new XElement("location");
                //     locationXml.SetAttributeValue("name", location.Location);

                //     foreach (Workshop workshop in location.Workshops)
                //     {
                //         XElement workshopXml = new XElement("workshop");
                //         workshopXml.SetAttributeValue("name", workshop.Name);
                //         workshopXml.SetAttributeValue("total-profit", CalculateTotalProfit(workshop));

                //         XElement participantsListXml = new XElement("participants");
                //         participantsListXml.SetAttributeValue("count", workshop.Participants.Count);

                //         foreach (Photographer participant in workshop.Participants)
                //         {
                //             XElement pariticipantXml = new XElement("participant");
                //             pariticipantXml.SetValue(participant.FullName);

                //             participantsListXml.Add(pariticipantXml);
                //         }

                //         workshopXml.Add(participantsListXml);
                //         locationXml.Add(workshopXml);
                //     }

                //     locationsListXml.Add(locationXml);
                // }

                // documentXml.Add(locationsListXml);
                // documentXml.Save("../../../datasets/workshops-by-location.xml");
            }
        }

        private static decimal CalculateTotalProfit(Workshop workshop)
        {
            {
                decimal totalProfit = (workshop.Participants.Count * workshop.PricePerParticipant) -
                                  ((workshop.Participants.Count * workshop.PricePerParticipant) * 0.2m);
                return totalProfit;
            }
        }
    }
}
