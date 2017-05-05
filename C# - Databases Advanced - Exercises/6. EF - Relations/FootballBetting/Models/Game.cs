namespace FootballBetting.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Game
    {
        public Game()
        {
            this.PlayerStatisticses = new HashSet<PlayerStatistic>();
            this.BetGames = new HashSet<BetGame>();
        }

        [Key]
        public int Id { get; set; }

        public DateTime DateTimeOfGame { get; set; }

        [Range(0, int.MaxValue)]
        public int HomeGoals { get; set; }

        [Range(0, int.MaxValue)]
        public int AwayGoals { get; set; }

        [Range(0, double.MaxValue)]
        public double HomeTeamWinBetRate { get; set; }

        [Range(0, double.MaxValue)]
        public double AwayTeamWinBetRate { get; set; }

        [Range(0, double.MaxValue)]
        public double DrawBetRate { get; set; }

        public int HomeTeamId { get; set; }

        public virtual Team HomeTeam { get; set; }

        public int AwayTeamId { get; set; }

        public virtual Team AwayTeam { get; set; }

        public int RoundId { get; set; }

        public virtual Round Round { get; set; }

        public int CompetitionId { get; set; }

        public virtual Competition Competition { get; set; }

        public virtual ICollection<BetGame> BetGames { get; set; }

        public virtual ICollection<PlayerStatistic> PlayerStatisticses { get; set; }
    }
}
