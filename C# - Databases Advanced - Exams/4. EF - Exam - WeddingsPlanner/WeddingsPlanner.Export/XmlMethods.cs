namespace WeddingsPlanner.Export
{
    using DTOs;
    using Data;
    using Models;
    using System.IO;
    using Utilities;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using System.Collections.Generic;

    class XmlMethods
    {
        private static void ExportXmlToFolder<TEntity>(TEntity entityType, string pathToExport)
        {
            XmlSerializer serializer = new XmlSerializer(entityType.GetType(), new XmlRootAttribute("venues"));
            StreamWriter writer = new StreamWriter(pathToExport);
            using (writer)
            {
                serializer.Serialize(writer, entityType);
            }
        }

        public static void ExportVenuesInSofia()
        {
            using (WeddingsPlannerContext context = new WeddingsPlannerContext())
            {
                // With 'Serializer'

                List<VenueDto> venuesInSofia = context.Venues
                    .Where(venue => venue.Weddings.Count >= 3 && venue.Town != "Sofia")
                    .Select(venue => new VenueDto
                    {
                        Name = venue.Name,
                        Capacity = venue.Capacity,
                        WeddingsCount = venue.Weddings.Count
                    })
                    .OrderBy(venue => venue.Capacity)
                    .ToList();

                ExportXmlToFolder(venuesInSofia, Constants.SofiaVenuesPath);

                // Without 'Serializer'

                //Venues which are not in Sofia because there are no venues in Sofia in the database
                //var venuesInSofia = context.Venues
                //    .Where(venue => venue.Weddings.Count >= 3 && venue.Town != "Sofia")
                //    .OrderBy(venue => venue.Capacity);

                //XDocument documentNode = new XDocument();

                //XElement venuesNode = new XElement("venues");
                //venuesNode.SetAttributeValue("town", "Sofia");

                //foreach (Venue venueInSofia in venuesInSofia)
                //{
                //    XElement venueNode = new XElement("venue");
                //    venueNode.SetAttributeValue("name", venueInSofia.Name);
                //    venueNode.SetAttributeValue("capacity", venueInSofia.Capacity);
                //    venueNode.SetElementValue("weddings-count", venueInSofia.Weddings.Count);

                //    venuesNode.Add(venueNode);
                //}

                //documentNode.Add(venuesNode);
                //documentNode.Save(Constants.SofiaVenuesPath);
            }
        }

        public static void ExportAgenciesByTown()
        {
            using (WeddingsPlannerContext context = new WeddingsPlannerContext())
            {
                // With 'Serializer'

                List<TownDto> agenciesByTown = context.Agencies
                    .Where(agency => agency.Town.Length >= 6)
                    .GroupBy(agency => agency.Town, agency => agency, (town, agencies) => new
                    {
                        Town = town,
                        Agencies = agencies.Where(agency => agency.Weddings.Count >= 2)
                    })
                    .Select(gr => new TownDto()
                    {
                        Name = gr.Town,
                        AgenciesDtos = gr.Agencies.Select(agency => new AgencyInTownDto()
                        {
                            Name = agency.Name,
                            Profit = agency.Weddings
                                        .Sum(wedding => wedding.Invitations
                                        .Where(inv => (inv.Present as Cash) != null)
                                        .Sum(inv => (inv.Present as Cash).Amount)
                                        ) * 0.2m,
                            Weddings = agency.Weddings.Select(wedding => new WeddingDto()
                            {
                                Cash = (wedding.Invitations
                                        .Where(inv => (inv.Present as Cash) != null)
                                        .Sum(inv => (decimal?)(inv.Present as Cash).Amount) ?? 0.0m),
                                Present = wedding.Invitations.Count(inv => (inv.Present as Gift) != null),
                                Bride = wedding.Bride.FirstName + " " + wedding.Bride.MiddleNameInitial + " " + wedding.Bride.LastName,
                                Bridegroom = wedding.Bridegroom.FirstName + " " + wedding.Bridegroom.MiddleNameInitial + " " + wedding.Bridegroom.LastName,
                                Guests = wedding.Invitations.Where(inv => inv.IsAttending).Select(guest => new GuestDto()
                                {
                                    Family = guest.Family.ToString(),
                                    Name = guest.Guest.FirstName + " " + guest.Guest.MiddleNameInitial + " " + guest.Guest.LastName,
                                }).ToList()
                            }).ToList()
                        }).ToList()
                    }).ToList();

                ExportXmlToFolder(agenciesByTown, Constants.AgenciesByTownPath);

                // Without 'Serializer'

                //var agenciesByTown = context.Agencies
                //    .Where(agency => agency.Town.Length >= 6)
                //    .GroupBy(agency => agency.Town, agency => agency, (town, agencies) => new
                //    {
                //        Town = town,
                //        Agencies = agencies.Where(agency => agency.Weddings.Count >= 2)
                //    });

                //XDocument documentNode = new XDocument();

                //XElement townsNode = new XElement("towns");

                //foreach (var agencyByTown in agenciesByTown)
                //{
                //    XElement townNode = new XElement("town");
                //    townNode.SetAttributeValue("name", agencyByTown.Town);

                //    XElement agenciesNode = new XElement("agencies");

                //    foreach (Agency agency in agencyByTown.Agencies)
                //    {
                //        XElement agencyNode = new XElement("agency");
                //        agencyNode.SetAttributeValue("name", agency.Name);
                //        agencyNode.SetAttributeValue("profit", GetAgencyProfit(agency.Weddings));

                //        foreach (Wedding wedding in agency.Weddings)
                //        {
                //            XElement weddingNode = new XElement("wedding");
                //            weddingNode.SetAttributeValue("cash", GetWeddingCash(wedding.Invitations));
                //            weddingNode.SetAttributeValue("presents", GetWeddingGiftsCount(wedding.Invitations));
                //            weddingNode.SetElementValue("bride", wedding.Bride.FullName);
                //            weddingNode.SetElementValue("bridegroom", wedding.Bridegroom.FullName);

                //            XElement guestsNode = new XElement("guests");

                //            foreach (Invitation invitation in wedding.Invitations.Where(inv => inv.IsAttending))
                //            {
                //                XElement guestNode = new XElement("guest");
                //                guestNode.SetAttributeValue("family", invitation.Family);
                //                guestNode.SetValue(invitation.Guest.FullName);

                //                guestsNode.Add(guestNode);
                //            }

                //            weddingNode.Add(guestsNode);
                //            agencyNode.Add(weddingNode);
                //        }

                //        agenciesNode.Add(agencyNode);
                //    }

                //    townNode.Add(agenciesNode);
                //    townsNode.Add(townNode);
                //}

                //documentNode.Add(townsNode);
                //documentNode.Save(Constants.AgenciesByTownPath);
            }
        }

        private static decimal GetAgencyProfit(ICollection<Wedding> Weddings)
        {
            decimal profit = Weddings
                .Sum(wedding => wedding.Invitations
                .Where(inv => (inv.Present as Cash) != null)
                .Sum(inv => (decimal?)(inv.Present as Cash).Amount) ?? 0.0m
                ) * 0.2m;

            return profit;
        }

        private static int GetWeddingGiftsCount(ICollection<Invitation> Invitations)
        {
            int giftsCount = Invitations.Count(inv => (inv.Present as Gift) != null);

            return giftsCount;
        }

        private static decimal GetWeddingCash(ICollection<Invitation> Invitations)
        {
            decimal cashAmount = Invitations
                                    .Where(inv => (inv.Present as Cash) != null)
                                    .Sum(inv => (decimal?)(inv.Present as Cash).Amount)  ?? 0.0m ;

            return cashAmount;
        }
    }
}
