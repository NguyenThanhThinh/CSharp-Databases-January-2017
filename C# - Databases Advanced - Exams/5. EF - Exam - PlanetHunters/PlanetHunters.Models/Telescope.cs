namespace PlanetHunters.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Telescope
    {
        public Telescope()
        {
            this.Discoveries = new HashSet<Discovery>();
        }

        //private double mirrorDiameter;

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Location { get; set; }

        [Range(0.0001f, float.MaxValue)]
        public float? MirrorDiameter { get; set; }

        //public double MirrorDiameter
        //{
        //    get
        //    {
        //        return this.mirrorDiameter;
        //    }
        //    set
        //    {
        //        double checkDiam = value;
        //        if (checkDiam > 0.0)
        //        {
        //            this.mirrorDiameter = value;
        //        }
        //        else
        //        {
        //            throw new Exception("Invalid Miror Diameter!");
        //        }
        //    }
        //}

        public virtual ICollection<Discovery> Discoveries { get; set; }
    }
}