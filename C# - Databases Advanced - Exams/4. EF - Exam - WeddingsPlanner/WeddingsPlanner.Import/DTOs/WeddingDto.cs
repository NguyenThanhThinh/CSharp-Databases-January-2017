namespace WeddingsPlanner.Import.DTOs
{
    using System;
    using System.Collections.Generic;

    public class WeddingDto
    {
        public WeddingDto()
        {
            this.Guests = new HashSet<GuestDto>();
        }

        public string Bride { get; set; }

        public string Bridegroom { get; set; }

        public DateTime? Date { get; set; }

        public string Agency { get; set; }

        public ICollection<GuestDto> Guests { get; set; }
    }
}