namespace PlanetHunters.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Star
    {
        private int temperature;

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Range(2400, int.MaxValue)]
        public int Temperature { get; set; }

        //[Required]
        //public int Temperature
        //{
        //    get
        //    {
        //        return this.temperature;
        //    }
        //    set
        //    {
        //        int checkTemp = value;
        //        if (checkTemp >= 2400)
        //        {
        //            this.temperature = value;
        //        }
        //        else
        //        {
        //            throw new Exception("Invalid Temperature (lower than 2400K)!");
        //        }
        //    }
        //}

        public int HostStarSystemId { get; set; }

        public virtual StarSystem HostStarSystem { get; set; }

        public int? DiscoveryId { get; set; }

        public virtual Discovery Discovery { get; set; }
    }
}