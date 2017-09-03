namespace Stations.Export.exportDtos
{
    using System;

    public class TrainDto
    {
        public string TrainNumber { get; set; }

        public int DelayedTimes { get; set; }

        public TimeSpan MaxDelayedTime { get; set; }
    }
}