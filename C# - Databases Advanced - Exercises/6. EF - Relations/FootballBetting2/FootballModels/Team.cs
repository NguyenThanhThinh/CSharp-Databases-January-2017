namespace FootballModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Team
    {
        public Team()
        {
            this.AwayGames = new HashSet<Game>();
            this.HomeGames = new HashSet<Game>();
            this.Players = new List<Player>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public byte[] Logo { get; set; }

        [RegularExpression("^[a-zA-Z]{3}$")]
        public string Initials { get; set; }

        public int PrimaryKitColourId { get; set; }

        public virtual Colour PrmaryKitColour { get; set; }

        public int SecondaryKitColourId { get; set; }

        public virtual Colour SecondaryKitColour { get; set; }

        public int TownId { get; set; }

        public virtual Town Town { get; set; }

        public decimal Budget { get; set; }

        public virtual ICollection<Game> AwayGames { get; set; }

        public virtual ICollection<Player> Players { get; set; }

        public virtual ICollection<Game> HomeGames { get; set; }
    }
}