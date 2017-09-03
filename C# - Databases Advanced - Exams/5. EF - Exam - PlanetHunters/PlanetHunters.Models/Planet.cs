namespace PlanetHunters.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Planet
    {
        //private double mass;

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [Range(0.0001f, float.MaxValue)]
        public float Mass { get; set; }

        //[Required]
        //public double Mass
        //{
        //    get
        //    {
        //        return this.mass;
        //    }
        //    set
        //    {
        //        double checkMass = value;
        //        if (checkMass > 0.0)
        //        {
        //            this.mass = value;
        //        }
        //        else
        //        {
        //            throw new Exception("Invalid Planet Mass!");
        //        }
        //    }
        //}

        public int HostStarSystemId { get; set; }

        public virtual StarSystem HostStarSystem { get; set; }

        public int? DiscoveryId { get; set; }

        public virtual Discovery Discovery { get; set; }
    }
}