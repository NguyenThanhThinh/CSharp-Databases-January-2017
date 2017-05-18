namespace Stations.Import.importDtos
{
    using Models;
    using System.Collections.Generic;

    public class TrainDto
    {
        public string TrainNumber { get; set; }

        public TrainType? Type { get; set; }

        public ICollection<SeatsClassPerTrainDto> Seats { get; set; }
    }
}
