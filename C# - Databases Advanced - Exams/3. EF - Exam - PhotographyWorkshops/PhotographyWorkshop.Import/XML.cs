using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using PhotographyWorkshop.Data;
using PhotographyWorkshop.Utilites;
using PhotographyWorkshops.Models;

namespace PhotographyWorkshop.ImportJson
{
    public class XML
    {
        public static void ImportAccessories()
        {
            XDocument documentXml = XDocument.Load(Constants.AccessoriesPath);

            //IEnumerable<XElement> accessoriesListXml = documentXml.Root?.Elements();
            IEnumerable<XElement> accessoriesListXml = documentXml.XPathSelectElements("accessories/accessory");

            using (PhotographyWorkshopContext context = new PhotographyWorkshopContext())
            {
                List<Accessory> accessoriesList = new List<Accessory>();

                foreach (XElement accessoryXml in accessoriesListXml)
                {
                    Accessory accessoryEntity = new Accessory()
                    {
                        Name = accessoryXml.Attribute("name")?.Value,
                        Owner = GetRandomOwner(context)
                    };

                    Console.WriteLine($"Successfully imported {accessoryEntity.Name}");
                    accessoriesList.Add(accessoryEntity);
                }

                context.Accessories.AddRange(accessoriesList);
                context.SaveChanges();
            }
        }

        public static void ImportWorkshops()
        {
            XDocument documentXml = XDocument.Load(Constants.WorkshopsPath);

            //IEnumerable<XElement> workshopsListXml = documentXml.Root?.Elements();
            IEnumerable<XElement> workshopsListXml = documentXml.XPathSelectElements("workshops/workshop");

            using (PhotographyWorkshopContext context = new PhotographyWorkshopContext())
            {
                List<Workshop> workshopsList = new List<Workshop>();

                foreach (XElement workshopXml in workshopsListXml)
                {
                    string name = workshopXml.Attribute("name")?.Value;
                    string location = workshopXml.Attribute("location")?.Value;
                    string priceAsString = workshopXml.Attribute("price")?.Value;
                    XElement trainerName = workshopXml.XPathSelectElement("trainer");

                    if (name == null || location == null || priceAsString == null || trainerName == null)
                    {
                        Console.WriteLine(Messages.InvalidDate);
                        continue;
                    }

                    Photographer trainer = context.Photographers
                        .FirstOrDefault(photographer => photographer.FirstName + " " + photographer.LastName == trainerName.Value);

                    DateTime? startDate = null;
                    DateTime? endDate = null;

                    string startDateAString = workshopXml.Attribute("start-date")?.Value;
                    if (startDateAString != null)
                    {
                        startDate = DateTime.Parse(startDateAString);
                    }

                    string endDateAsString = workshopXml.Attribute("end-date")?.Value;
                    if (endDateAsString != null)
                    {
                        endDate = DateTime.Parse(endDateAsString);
                    }

                    var workshopEntity = new Workshop()
                    {
                        Name = name,
                        StartDate = startDate,
                        EndDate = endDate,
                        Location = location,
                        PricePerParticipant = decimal.Parse(priceAsString),
                        Trainer = trainer
                    };
                    
                    IEnumerable<XElement> participantsListXml = workshopXml.XPathSelectElements("participants/participant");

                    foreach (XElement participantXml in participantsListXml)
                    {
                        string firstName = participantXml.Attribute("first-name")?.Value;
                        string lastName = participantXml.Attribute("last-name")?.Value;

                        if (firstName == null || lastName == null)
                        {
                            Console.WriteLine(Messages.InvalidDate);
                            continue;
                        }

                        Photographer participant = context.Photographers
                            .FirstOrDefault(photographer => photographer.FirstName == firstName && photographer.LastName == lastName);

                        if (participant == null)
                        {
                            Console.WriteLine(Messages.InvalidDate);
                            continue;
                        }

                        workshopEntity.Participants.Add(participant);
                    }

                    Console.WriteLine($"Successfully imported {workshopEntity.Name}");
                    workshopsList.Add(workshopEntity);
                }

                context.Workshops.AddRange(workshopsList);
                context.SaveChanges();
            }
        }

        private static Photographer GetRandomOwner(PhotographyWorkshopContext context)
        {
            Random rnd = new Random();
            int randomId = rnd.Next(1, context.Photographers.Count() + 1);
            Photographer photographer = context.Photographers.Find(randomId);
            return photographer;
        }
    }
}
