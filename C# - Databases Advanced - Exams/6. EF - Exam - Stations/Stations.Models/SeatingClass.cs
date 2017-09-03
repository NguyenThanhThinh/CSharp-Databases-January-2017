namespace Stations.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class SeatingClass
    {
        public SeatingClass()
        {
            this.TrainSeats = new HashSet<TrainSeat>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(2)]
        public string Abbreviation { get; set; }

        public virtual ICollection<TrainSeat> TrainSeats { get; set; }
    }
}