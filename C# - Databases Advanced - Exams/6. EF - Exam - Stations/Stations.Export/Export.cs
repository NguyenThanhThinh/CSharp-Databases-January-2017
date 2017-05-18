namespace Stations.Export
{
    using Data;
    using Models;
    using System;
    using System.IO;
    using exportDtos;
    using AutoMapper;
    using System.Linq;
    using System.Xml.Linq;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.Collections.Generic;
    using AutoMapper.QueryableExtensions;

    public class Export
    {
        private readonly HelperMethods queryHelper;

        public Export(HelperMethods queryHelper)
        {
            this.queryHelper = queryHelper;

            Mapper.Initialize(
                cfg =>
                {
                    cfg.CreateMap<IGrouping<string, TripDto>, TrainDto>()
                        .ForMember(dest => dest.TrainNumber, opt => opt.MapFrom(src => src.Key))
                        .ForMember(dest => dest.DelayedTimes, opt => opt.MapFrom(src => src.Count()))
                        .ForMember(dest => dest.MaxDelayedTime, opt => opt.MapFrom(src => GetHighestTime(src)));

                    cfg.CreateMap<Ticket, CardTicketDto>()
                        .ForMember(dest => dest.OriginStation, opt => opt.MapFrom(src => src.Trip.OriginStation.Name))
                        .ForMember(dest => dest.DestinationStation, opt => opt.MapFrom(src => src.Trip.DestinationStation.Name))
                        .ForMember(dest => dest.DepartureTime, opt => opt.MapFrom(src => src.Trip.DepartureTime));

                    cfg.CreateMap<CustomerCard, CardDto>()
                        .ForMember(dest => dest.Tickets, opt => opt.MapFrom(src => src.BoughtTickets));
                });
        }

        public void ExportDelayedTrains(string path, string dateAsString)
        {
            DateTime date = ParseDate(dateAsString);

            List<TrainDto> delayedTrips = this.queryHelper
                .Filter<Trip>(trip => trip.Status == TripStatus.Delayed && trip.DepartureTime <= date)
                .Select(trip => new TripDto
                            {
                                TrainNumber = trip.Train.TrainNumber,
                                TimeDifference = trip.TimeDifference
                            })
                .GroupBy(trip => trip.TrainNumber)
                .ToList()
                .AsQueryable()
                .ProjectTo<TrainDto>()
                .OrderByDescending(t => t.DelayedTimes)
                .ThenByDescending(t => t.MaxDelayedTime)
                .ThenBy(t => t.TrainNumber)
                .ToList();

            string json = JsonConvert.SerializeObject(delayedTrips, Formatting.Indented);

            File.WriteAllText($"{path}delayed-trips-by-{date.Date:dd-MM-yyyy}.json", json);
        }

        public void ExportCardsTicket(string path, string cardType)
        {
            XDocument document = new XDocument();

            CardType type = (CardType)Enum.Parse(typeof(CardType), cardType);

            List<CardDto> cardsDto = this.queryHelper
                .Filter<CustomerCard>(c => c.Type == type)
                .ProjectTo<CardDto>()
                .Where(c => c.Tickets.Count > 0)
                .OrderBy(c => c.Name)
                .ToList();

            XElement cardsNode = new XElement("Cards");

            foreach (CardDto cardDto in cardsDto)
            {
                XElement cardNode = new XElement("Card");
                cardNode.SetAttributeValue("name", cardDto.Name);
                cardNode.SetAttributeValue("type", cardDto.Type.ToString());

                XElement ticketsNode = new XElement("Tickets");
                foreach (CardTicketDto ticketDto in cardDto.Tickets)
                {
                    XElement ticketElement = new XElement("Ticket");
                    ticketElement.SetElementValue("OriginStation", ticketDto.OriginStation);
                    ticketElement.SetElementValue("DestinationStation", ticketDto.DestinationStation);
                    ticketElement.SetElementValue("DepartureTime", ticketDto.DepartureTime.ToString("dd/MM/yyyy HH:mm"));

                    ticketsNode.Add(ticketElement);
                }

                cardNode.Add(ticketsNode);
                cardsNode.Add(cardNode);
            }

            document.Add(cardsNode);
            document.Save($"{path}tickets-bought-withCardType-{cardType}.xml");
        }

        private static TimeSpan GetHighestTime(IGrouping<string, TripDto> grouping)
        {
            TimeSpan result = grouping
                .Where(trip => trip.TimeDifference.HasValue)
                .OrderByDescending(trip => trip.TimeDifference)
                .Select(trip => trip.TimeDifference.Value)
                .FirstOrDefault();

            return result;
        }

        private static DateTime ParseDate(string dateAsString)
        {
            return DateTime.ParseExact(dateAsString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
    }
}
