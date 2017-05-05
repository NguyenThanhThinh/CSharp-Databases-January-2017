namespace FootballModels
{
    using System;
    using System.Collections.Generic;

    public class Game
    {
        public Game()
        {
            this.BetGames = new HashSet<BetGame>();
            this.PlayerStatistics = new HashSet<PlayerStatistic>();
        }

        public int Id { get; set; }

        public int HomeTeamId { get; set; }

        public virtual Team HomeTeam { get; set; }

        public int AwayTeamId { get; set; }

        public virtual Team AwayTeam { get; set; }

        public int HomeGoals { get; set; }

        public int AwayGoals { get; set; }

        public DateTime GameDateTime { get; set; }

        public float HomeTeamWinBetRate { get; set; }
        
        public float AwayTeamWinBetRate { get; set; }

        public float DrawGameBetRate { get; set; }

        public int RoundId { get; set; }

        public virtual Round Round { get; set; }
        
        public int CompetitionId { get; set; }

        public virtual Competition Competition { get; set; }

        public virtual ICollection<BetGame> BetGames { get; set; }

        public virtual ICollection<PlayerStatistic> PlayerStatistics { get; set; }
    }
}
