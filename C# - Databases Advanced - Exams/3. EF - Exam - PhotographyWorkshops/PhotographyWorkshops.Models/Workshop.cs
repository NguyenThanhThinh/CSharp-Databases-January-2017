namespace PhotographyWorkshops.Models
{
    using System;
    using Microsoft.Build.Framework;
    using System.Collections.Generic;

    public class Workshop
    {
        public Workshop()
        {
            this.Participants = new HashSet<Photographer>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        public string Location { get; set; }

        public decimal PricePerParticipant { get; set; }

        public int TrainerId { get; set; }
        
        public virtual Photographer Trainer { get; set; }

        public virtual ICollection<Photographer> Participants { get; set; }
    }
}
