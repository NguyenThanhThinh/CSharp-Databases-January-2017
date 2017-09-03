namespace FootballBetting.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Country
    {
        public Country()
        {
            this.Continent = new HashSet<Continent>();
            this.Towns = new HashSet<Town>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Continent> Continent { get; set; }

        public virtual ICollection<Town> Towns { get; set; }
    }
}