namespace FootballBetting.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Color
    {
        public Color()
        {
            this.HomeTeamColors = new HashSet<Team>();
            this.AwayTeamColors = new HashSet<Team>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Team> HomeTeamColors { get; set; }

        public ICollection<Team> AwayTeamColors { get; set; }
    }
}