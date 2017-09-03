namespace FootballModels
{
    using System.Collections.Generic;

    public class Country
    {
        public Country()
        {
            this.Continents = new HashSet<Continent>();
            this.Towns = new HashSet<Town>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Continent> Continents { get; set; }

        public virtual ICollection<Town> Towns { get; set; }
    }
}