namespace Stations.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public enum TripStatus
    {
        OnTime,
        Delayed,
        Early
    }

    public class Trip
    {
        public int Id { get; set; }

        [Required]
        public int OriginStationId { get; set; }

        public virtual Station OriginStation { get; set; }

        [Required]
        public int DestinationStationId { get; set; }

        public virtual Station DestinationStation { get; set; }

        [Required]
        public DateTime ArrivalTime { get; set; }

        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public int TrainId { get; set; }

        public virtual Train Train { get; set; }

        public TripStatus Status { get; set; } 

        public TimeSpan? TimeDifference { get; set; }
    }
}
