namespace WeddingsPlanner.Import
{
    using Data;
    using DTOs;
    using Models;
    using Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using Utilities;

    internal class XmlMethods
    {
        public static void ImportVenues()
        {
            using (WeddingsPlannerContext context = new WeddingsPlannerContext())
            {
                XDocument documentNode = XDocument.Load(Constants.VenuesPath);

                IEnumerable<XElement> venuesNode = documentNode.XPathSelectElements("venues/venue");

                foreach (XElement venueNode in venuesNode)
                {
                    string venueNodeName = venueNode.Attribute("name")?.Value;
                    int venueNodeNameCapacity = int.Parse(venueNode.Element("capacity").Value);
                    string venueNodeTown = venueNode.Element("town")?.Value;

                    VenueDto venueDto = new VenueDto()
                    {
                        Name = venueNodeName,
                        Capacity = venueNodeNameCapacity,
                        Town = venueNodeTown
                    };

                    Venue venueEntity = new Venue()
                    {
                        Name = venueDto.Name,
                        Capacity = venueDto.Capacity,
                        Town = venueDto.Town
                    };

                    try
                    {
                        context.Venues.Add(venueEntity);
                        context.SaveChanges();
                        Console.WriteLine($"Successfully imported {venueEntity.Name}");
                    }
                    catch (DbEntityValidationException)
                    {
                        context.Venues.Remove(venueEntity);
                        Console.WriteLine(Messages.InvalidData);
                    }
                }

                Random random = new Random();
                foreach (Wedding wedding in context.Weddings.ToList())
                {
                    int randomId = random.Next(1, context.Venues.Count() + 1);
                    Venue venue = context.Venues.Find(randomId);

                    wedding.Venues.Add(venue);

                    randomId = random.Next(1, context.Venues.Count() + 1);
                    venue = context.Venues.Find(randomId);
                    wedding.Venues.Add(venue);
                }

                context.SaveChanges();
            }
        }

        public static void ImportPresents()
        {
            using (WeddingsPlannerContext context = new WeddingsPlannerContext())
            {
                XDocument documentNode = XDocument.Load(Constants.PresentsPath);

                IEnumerable<XElement> presentsNode = documentNode.XPathSelectElements("presents/present");

                foreach (XElement presentNode in presentsNode)
                {
                    string type = presentNode.Attribute("type")?.Value;
                    string id = presentNode.Attribute("invitation-id")?.Value;
                    string name = presentNode.Attribute("present-name")?.Value;
                    string size = presentNode.Attribute("size")?.Value;
                    string amount = presentNode.Attribute("amount")?.Value;

                    if (type == null || id == null)
                    {
                        Console.WriteLine(Messages.InvalidData);
                        continue;
                    }

                    int invitationId = int.Parse(presentNode.Attribute("invitation-id").Value);
                    int invitationsCount = context.Invitations.Count();

                    if (invitationId < 0 || invitationId > invitationsCount)
                    {
                        Console.WriteLine(Messages.InvalidData);
                        continue;
                    }

                    Invitation invitation = context.Invitations.FirstOrDefault(inv => inv.Id == invitationId);
                    if (invitation == null)
                    {
                        Console.WriteLine(Messages.InvalidData);
                        continue;
                    }

                    if (type == "cash")
                    {
                        if (amount == null)
                        {
                            Console.WriteLine(Messages.InvalidData);
                            continue;
                        }

                        PresentDto cashDto = new PresentDto()
                        {
                            Amount = decimal.Parse(amount),
                            InvitationId = invitationId
                        };

                        Cash cashEntity = new Cash()
                        {
                            Amount = cashDto.Amount,
                            Invitation = context.Invitations.Find(cashDto.InvitationId)
                        };

                        try
                        {
                            context.Presents.Add(cashEntity);
                            invitation.PresentId = cashEntity.InvitationId;
                            context.SaveChanges();
                            Console.WriteLine($"Succesfully imported cash from {invitation.Guest.FullName}!");
                        }
                        catch (DbEntityValidationException)
                        {
                            context.Presents.Remove(cashEntity);
                            Console.WriteLine(Messages.InvalidData);
                        }
                    }
                    else if (type == "gift")
                    {
                        if (name == null)
                        {
                            Console.WriteLine(Messages.InvalidData);
                            continue;
                        }

                        PresentSize presentSize = PresentSize.NotSpecified;

                        if (size != null)
                        {
                            bool isPresentSizeValid = Enum.TryParse(size, out presentSize);
                            if (!isPresentSizeValid)
                            {
                                Console.WriteLine(Messages.InvalidData);
                                continue;
                            }
                        }

                        PresentDto giftDto = new PresentDto()
                        {
                            InvitationId = invitationId,
                            PresentName = name,
                            Size = presentSize
                        };

                        Gift giftEntity = new Gift()
                        {
                            Name = giftDto.PresentName,
                            Invitation = context.Invitations.Find(giftDto.InvitationId),
                            PresentSize = giftDto.Size
                        };

                        try
                        {
                            context.Presents.Add(giftEntity);
                            invitation.PresentId = giftEntity.InvitationId;
                            context.SaveChanges();
                            Console.WriteLine($"Succesfully imported gift from {invitation.Guest.FullName}!");
                        }
                        catch (DbEntityValidationException)
                        {
                            context.Presents.Remove(giftEntity);
                            Console.WriteLine(Messages.InvalidData);
                        }
                    }
                }
            }
        }
    }
}