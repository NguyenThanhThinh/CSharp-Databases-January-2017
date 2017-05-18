namespace Stations.Import.importDtos
{
    using Models;
    using System;

    public class TripDto
    {
        public string OriginStation { get; set; }

        public string DestinationStation { get; set; }

        public DateTime? DepartureTime { get; set; }

        public DateTime? ArrivalTime { get; set; }

        public string Train { get; set; }

        public TripStatus? Status { get; set; }

        public string TimeDifference { get; set; }
    }
}
