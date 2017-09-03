namespace WeddingsPlanner.Export
{
    using Data;
    using DTOs;
    using Models.Enums;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Utilities;

    internal class JsonMethods
    {
        public static void ExportJsonToFolder<TEntity>(TEntity entityType, string pathToExport)
        {
            string json = JsonConvert.SerializeObject(entityType, Formatting.Indented);
            File.WriteAllText(pathToExport, json);
        }

        public static void ExportOrderedAgencies()
        {
            using (WeddingsPlannerContext context = new WeddingsPlannerContext())
            {
                List<OrderedAgenciesDto> orderedAgencies = context.Agencies
                    .OrderByDescending(agency => agency.EmployeesCount)
                    .ThenBy(agency => agency.Name)
                    .Select(agency => new OrderedAgenciesDto
                    {
                        Name = agency.Name,
                        Count = agency.EmployeesCount,
                        Town = agency.Town
                    })
                    .ToList();

                ExportJsonToFolder(orderedAgencies, Constants.AgenciesOrderedPath);
            }
        }

        public static void ExportGuestList()
        {
            using (WeddingsPlannerContext context = new WeddingsPlannerContext())
            {
                List<GuestsListDto> guestsList = context.Weddings
                    .Select(wedding => new GuestsListDto
                    {
                        Bride = wedding.Bride.FirstName + " " + wedding.Bride.MiddleNameInitial + " " + wedding.Bride.LastName,
                        Bridegroom = wedding.Bridegroom.FirstName + " " + wedding.Bridegroom.MiddleNameInitial + " " + wedding.Bridegroom.LastName,
                        AgencyDto = new AgencyDto
                        {
                            Name = wedding.Agency.Name,
                            Town = wedding.Agency.Town
                        },
                        InvitedGuests = wedding.Invitations.Count,
                        BrideGuests = wedding.Invitations.Count(inv => inv.Family == Family.Bride),
                        BridegroomGuests = wedding.Invitations.Count(inv => inv.Family == Family.Bridegroom),
                        AttendingGuests = wedding.Invitations.Count(inv => inv.IsAttending),
                        GuestsNames = wedding.Invitations
                            .Where(inv => inv.IsAttending)
                            .Select(inv => inv.Guest.FirstName + " " + inv.Guest.MiddleNameInitial + " " + inv.Guest.LastName)
                            .ToList()
                    })
                    .OrderByDescending(wedding => wedding.InvitedGuests)
                    .ThenBy(wedding => wedding.AttendingGuests)
                    .ToList();

                ExportJsonToFolder(guestsList, Constants.GuestsListPath);
            }
        }
    }
}