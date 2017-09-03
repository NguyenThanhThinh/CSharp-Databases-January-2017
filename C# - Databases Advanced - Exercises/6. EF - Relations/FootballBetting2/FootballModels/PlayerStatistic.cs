namespace FootballModels
{
    public class PlayerStatistic
    {
        public int GameId { get; set; }

        public virtual Game Game { get; set; }

        public int PlayerId { get; set; }

        public virtual Player Player { get; set; }

        public int ScoredGoals { get; set; }

        public int PlayerAssists { get; set; }

        public int PlayedMinutes { get; set; }
    }
}