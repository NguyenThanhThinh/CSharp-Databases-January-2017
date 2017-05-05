namespace PlanetHunters.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Astronomer
    {
        public Astronomer()
        {
            this.DiscoveriesMade = new HashSet<Discovery>();
            this.DiscoveriesObserved = new HashSet<Discovery>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        public virtual ICollection<Discovery> DiscoveriesMade { get; set; }

        public virtual ICollection<Discovery> DiscoveriesObserved { get; set; }
    }
}
