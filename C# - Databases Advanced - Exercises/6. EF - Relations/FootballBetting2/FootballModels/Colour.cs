namespace FootballModels
{
    using System.Collections.Generic;

    public class Colour
    {
        public Colour()
        {
            this.PrimaryColorTeams = new HashSet<Team>();
            this.SecondaryColorTeams = new HashSet<Team>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Team> PrimaryColorTeams { get; set; }

        public virtual ICollection<Team> SecondaryColorTeams { get; set; }
    }
}
