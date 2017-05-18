namespace Stations.Export.exportDtos
{
    using Models;
    using System.Collections.Generic;

    public class CardDto
    {
        public CardDto()
        {
            this.Tickets = new List<CardTicketDto>();
        }

        public string Name { get; set; }

        public CardType Type { get; set; }

        public ICollection<CardTicketDto> Tickets { get; set; }
    }
}
