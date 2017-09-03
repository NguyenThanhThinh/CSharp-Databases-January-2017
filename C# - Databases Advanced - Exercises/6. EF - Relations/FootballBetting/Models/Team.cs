namespace FootballBetting.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Team
    {
        public Team()
        {
            this.Players = new HashSet<Player>();
            this.HomeGames = new HashSet<Game>();
            this.AwayGames = new HashSet<Game>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public byte[] Logo { get; set; }

        public decimal Budget { get; set; }

        [Required]
        [MaxLength(3)]
        public string ThreeLetterInitials { get; set; }

        public int PrimaryKitColorId { get; set; }

        [Required]
        public virtual Color PrimaryKitColor { get; set; }

        public int SecondaryKitColorId { get; set; }

        [Required]
        public virtual Color SecondaryKitColor { get; set; }

        public int TownId { get; set; }

        public virtual Town Town { get; set; }

        public virtual ICollection<Player> Players { get; set; }

        public virtual ICollection<Game> HomeGames { get; set; }

        public virtual ICollection<Game> AwayGames { get; set; }
    }
}