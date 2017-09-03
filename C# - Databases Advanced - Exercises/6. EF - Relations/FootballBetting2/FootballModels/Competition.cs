namespace FootballModels
{
    using System.Collections.Generic;

    public class Competition
    {
        public Competition()
        {
            this.Games = new HashSet<Game>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int CompetitionTypeId { get; set; }

        public virtual CompetitionType CompetitionType { get; set; }

        public virtual ICollection<Game> Games { get; set; }
    }
}