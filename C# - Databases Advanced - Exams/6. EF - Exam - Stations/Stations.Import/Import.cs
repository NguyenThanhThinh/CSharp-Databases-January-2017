namespace Stations.Import
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using importDtos;
    using Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;

    public class Import
    {
        private const string ErrorMessage = "Invalid data format.";
        private const string RecordSuccessfullyImported = "Record {0} successfully imported.";

        private readonly HelperMethods helperMethods;

        public Import(HelperMethods helperMethods)
        {
            this.helperMethods = helperMethods;

            Mapper.Initialize(
                cfg =>
                {
                    cfg.CreateMap<SeatingClassDto, SeatingClass>();
                    cfg.CreateMap<SeatsClassPerTrainDto, SeatingClass>();
                    cfg.CreateMap<TrainDto, Train>();
                });
        }

        public void ImportStations(string json)
        {
            IEnumerable<Station> stations = JsonConvert.DeserializeObject<IEnumerable<Station>>(json);

            List<Station> validStations = new List<Station>();

            foreach (Station station in stations)
            {
                {
                    if (string.IsNullOrEmpty(station.Town))
                    {
                        station.Town = station.Name;
                    }

                    if (!this.helperMethods.IsEntityValid(station) || validStations.Any(s => s.Name == station.Name))
                    {
                        Console.WriteLine(ErrorMessage);
                    }
                    else
                    {
                        validStations.Add(station);
                        Console.WriteLine(RecordSuccessfullyImported, station.Name);
                    }
                }
            }

            this.helperMethods.AddRange(validStations);
        }

        public void ImportClasses(string json)
        {
            IEnumerable<SeatingClass> classes = JsonConvert.DeserializeObject<IEnumerable<SeatingClassDto>>(json)
                .AsQueryable()
                .ProjectTo<SeatingClass>();

            List<SeatingClass> validClasses = new List<SeatingClass>();

            foreach (SeatingClass seatingClass in classes)
            {
                // Class is invalid, existing or about to be added.
                if (!this.helperMethods.IsEntityValid(seatingClass) ||
                    this.helperMethods.IsEntityExisting<SeatingClass>(sc => sc.Name == seatingClass.Name || sc.Abbreviation == seatingClass.Abbreviation) ||
                    validClasses.Any(cl => cl.Name == seatingClass.Name || cl.Abbreviation == seatingClass.Abbreviation))
                {
                    Console.WriteLine(ErrorMessage);
                }
                else
                {
                    validClasses.Add(seatingClass);
                    Console.WriteLine(RecordSuccessfullyImported, seatingClass.Name);
                }
            }

            this.helperMethods.AddRange(validClasses);
        }

        public void ImportTrains(string json)
        {
            IEnumerable<TrainDto> trainDtos = JsonConvert.DeserializeObject<List<TrainDto>>(json);

            List<Train> validTrains = new List<Train>();

            foreach (TrainDto trainDto in trainDtos)
            {
                Train train = Mapper.Instance.Map<Train>(trainDto);

                // Train is invalid, existing or to be added.
                if (!this.helperMethods.IsEntityValid(train) ||
                    this.helperMethods.IsEntityExisting<Train>(t => t.TrainNumber == train.TrainNumber) ||
                    validTrains.Any(t => t.TrainNumber == train.TrainNumber))
                {
                    Console.WriteLine(ErrorMessage);
                    continue;
                }

                IEnumerable<SeatingClass> seats = Mapper.Instance.Map<List<SeatingClass>>(trainDto.Seats);

                if (seats != null)
                {
                    // Seat class is invalid or not existing.
                    if (seats.Any(s => !this.helperMethods.IsEntityValid(s)) ||
                        seats.Any(s => !this.helperMethods.IsEntityExisting<SeatingClass>(st => st.Name == s.Name &&
                                                                                        st.Abbreviation == s.Abbreviation)))
                    {
                        Console.WriteLine(ErrorMessage);
                        continue;
                    }

                    List<TrainSeat> trainSeats = trainDto.Seats.Select(s => new TrainSeat
                    {
                        SeatingClass = this.helperMethods.SingleOrDefault<SeatingClass>(sc => sc.Name == s.Name),
                        Quantity = s.Quantity ?? -1
                    })
                    .ToList();

                    if (trainSeats.Any(tr => !this.helperMethods.IsEntityValid(tr)))
                    {
                        Console.WriteLine(ErrorMessage);
                        continue;
                    }

                    foreach (TrainSeat trainSeat in trainSeats)
                    {
                        train.TrainSeats.Add(trainSeat);
                    }
                }

                Console.WriteLine(RecordSuccessfullyImported, train.TrainNumber);
                validTrains.Add(train);
            }

            this.helperMethods.AddRange(validTrains);
        }

        public void ImportTrips(string json)
        {
            List<TripDto> tripsDtos = JsonConvert.DeserializeObject<List<TripDto>>(json, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy HH:mm" });

            List<Trip> validTrips = new List<Trip>();

            foreach (TripDto tripDto in tripsDtos)
            {
                if (tripDto.ArrivalTime == null || tripDto.DepartureTime == null || tripDto.DepartureTime > tripDto.ArrivalTime)
                {
                    Console.WriteLine(ErrorMessage);
                    continue;
                }

                if (!this.helperMethods.IsEntityExisting<Train>(t => t.TrainNumber == tripDto.Train) ||
                    !this.helperMethods.IsEntityExisting<Station>(st => st.Name == tripDto.OriginStation) ||
                    !this.helperMethods.IsEntityExisting<Station>(st => st.Name == tripDto.DestinationStation))
                {
                    Console.WriteLine(ErrorMessage);
                    continue;
                }

                TimeSpan timeDifference;

                if (tripDto.TimeDifference != null && !TimeSpan.TryParseExact(tripDto.TimeDifference, @"hh\:mm", CultureInfo.InvariantCulture, out timeDifference))
                {
                    Console.WriteLine(ErrorMessage);
                    continue;
                }

                Trip trip = this.CreateTrip(tripDto);

                if (!this.helperMethods.IsEntityValid(trip))
                {
                    Console.WriteLine(ErrorMessage);
                    continue;
                }

                validTrips.Add(trip);
                Console.WriteLine($"Trip from {tripDto.OriginStation} to {tripDto.DestinationStation} imported.");
            }

            this.helperMethods.AddRange(validTrips);
        }

        public void ImportCards(string xml)
        {
            XDocument xmlDocument = XDocument.Parse(xml);

            //XElement root = xmlDocument.Element("Cards");
            IEnumerable<XElement> cardsNode = xmlDocument.XPathSelectElements("Cards/Card");

            List<CustomerCard> validCards = new List<CustomerCard>();

            foreach (XElement cardNode in cardsNode)
            {
                string name = cardNode.Element("Name").Value;
                int age = int.Parse(cardNode.Element("Age").Value);

                CardType cardType =
                    cardNode.Element("CardType") == null
                    ? CardType.Normal
                    : (CardType)Enum.Parse(typeof(CardType), cardNode.Element("CardType").Value);

                CustomerCard card = new CustomerCard
                {
                    Name = name,
                    Age = age,
                    Type = cardType
                };

                if (!this.helperMethods.IsEntityValid(card))
                {
                    Console.WriteLine(ErrorMessage);
                    continue;
                }

                validCards.Add(card);
                Console.WriteLine($"Record {card.Name} successfully imported.");
            }

            this.helperMethods.AddRange(validCards);
        }

        public void ImportTickets(string xml)
        {
            XDocument xmlDocument = XDocument.Parse(xml);

            //XElement root = xmlDocument.Element("Tickets");
            IEnumerable<XElement> ticketsNode = xmlDocument.XPathSelectElements("Tickets/Ticket");

            List<Ticket> validTickets = new List<Ticket>();

            foreach (XElement ticketNode in ticketsNode)
            {
                decimal price = decimal.Parse(ticketNode.Attribute("price").Value);

                if (price <= 0m)
                {
                    Console.WriteLine(ErrorMessage);
                    continue;
                }

                Ticket ticket = new Ticket
                {
                    Price = price
                };

                string seat = ticketNode.Attribute("seat")?.Value;

                if (!this.IsSeatNumberValid(seat) ||
                    !this.helperMethods.IsEntityExisting<SeatingClass>(sc => sc.Abbreviation == seat.Substring(0, 2)))
                {
                    Console.WriteLine(ErrorMessage);
                    continue;
                }

                XElement tripNode = ticketNode.Element("Trip");

                string originStationName = tripNode?.Element("OriginStation")?.Value;
                string destinationStationName = tripNode?.Element("DestinationStation")?.Value;

                DateTime departureTime = DateTime.ParseExact(tripNode.Element("DepartureTime").Value, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                Trip trip = this.helperMethods
                    .SingleOrDefault<Trip>(t =>
                        t.OriginStation.Name == originStationName &&
                        t.DestinationStation.Name == destinationStationName &&
                        t.DepartureTime == departureTime);

                if (trip == null)
                {
                    Console.WriteLine(ErrorMessage);
                    continue;
                }

                XElement cardNode = ticketNode.Element("Card");

                if (cardNode != null)
                {
                    string cardName = cardNode.Attribute("Name").Value;

                    CustomerCard card = this.helperMethods.SingleOrDefault<CustomerCard>(c => c.Name == cardName);

                    if (card == null)
                    {
                        Console.WriteLine(ErrorMessage);
                        continue;
                    }

                    ticket.CustomerCard = card;
                }

                if (!this.IsSeatPlaceValid(trip.Train, seat))
                {
                    Console.WriteLine(ErrorMessage);
                    continue;
                }

                ticket.SeatingPlace = seat;
                ticket.Trip = trip;

                validTickets.Add(ticket);
                Console.WriteLine($"Ticket from {originStationName} to {destinationStationName} departing at {departureTime:dd/MM/yyyy HH:mm} imported.");
            }

            this.helperMethods.AddRange(validTickets);
        }

        private Trip CreateTrip(TripDto tripDto)
        {
            Trip trip = new Trip
            {
                ArrivalTime = tripDto.ArrivalTime.Value,
                DepartureTime = tripDto.DepartureTime.Value,
                Train = this.helperMethods.SingleOrDefault<Train>(t => t.TrainNumber == tripDto.Train),
                OriginStation = this.helperMethods.SingleOrDefault<Station>(st => st.Name == tripDto.OriginStation),
                DestinationStation = this.helperMethods.SingleOrDefault<Station>(st => st.Name == tripDto.DestinationStation),
                Status = tripDto.Status ?? TripStatus.OnTime
            };

            if (!string.IsNullOrEmpty(tripDto.TimeDifference))
            {
                trip.TimeDifference = TimeSpan.ParseExact(tripDto.TimeDifference, @"hh\:mm", CultureInfo.InvariantCulture);
            }

            return trip;
        }

        private bool IsSeatNumberValid(string seat)
        {
            int parsedNumber;
            bool isNumber = int.TryParse(seat.Substring(2), out parsedNumber);

            if (!isNumber)
            {
                return false;
            }

            if (parsedNumber <= 0)
            {
                return false;
            }

            return true;
        }

        private bool IsSeatPlaceValid(Train train, string seat)
        {
            string abbreviation = seat.Substring(0, 2);
            int seatNumber = int.Parse(seat.Substring(2));

            return train.TrainSeats.Any(ts => ts.SeatingClass.Abbreviation == abbreviation && ts.Quantity >= seatNumber);
        }
    }
}