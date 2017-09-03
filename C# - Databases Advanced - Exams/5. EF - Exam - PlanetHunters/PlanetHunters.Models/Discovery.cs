namespace PlanetHunters.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Discovery
    {
        public Discovery()
        {
            this.Stars = new HashSet<Star>();
            this.Planets = new HashSet<Planet>();
            this.Pioneers = new HashSet<Astronomer>();
            this.Observers = new HashSet<Astronomer>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "DATE")]
        public DateTime DateMade { get; set; }

        public int TelesopeUsedId { get; set; }

        public virtual Telescope TelesopeUsed { get; set; }

        public virtual ICollection<Star> Stars { get; set; }

        public virtual ICollection<Planet> Planets { get; set; }

        // It's better to use the 'modelbuilder'
        //[InverseProperty("DiscoveriesMade")]
        public virtual ICollection<Astronomer> Pioneers { get; set; }

        // It's better to use the 'modelbuilder'
        //[InverseProperty("DiscoveriesObserved")]
        public virtual ICollection<Astronomer> Observers { get; set; }
    }
}