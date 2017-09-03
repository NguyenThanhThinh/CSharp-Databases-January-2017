namespace Stations.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Station
    {
        public Station()
        {
            this.TripsFrom = new HashSet<Trip>();
            this.TripsTo = new HashSet<Trip>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Town { get; set; }

        public ICollection<Trip> TripsFrom { get; set; }

        public ICollection<Trip> TripsTo { get; set; }
    }
}