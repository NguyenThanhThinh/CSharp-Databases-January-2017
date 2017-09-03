namespace Stations.Export.exportDtos
{
    using System;

    public class TripDto
    {
        public string TrainNumber { get; set; }

        public TimeSpan? TimeDifference { get; set; }
    }
}