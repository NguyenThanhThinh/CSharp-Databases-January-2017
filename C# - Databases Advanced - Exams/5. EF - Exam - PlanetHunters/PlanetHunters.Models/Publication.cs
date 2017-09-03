namespace PlanetHunters.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Publication
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "DATE")]
        public DateTime ReleaseDate { get; set; }

        public int DiscoveryId { get; set; }

        public virtual Discovery Discovery { get; set; }
    }
}