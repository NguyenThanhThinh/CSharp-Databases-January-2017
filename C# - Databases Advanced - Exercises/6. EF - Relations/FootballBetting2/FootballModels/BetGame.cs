namespace FootballModels
{
    public class BetGame
    {
        public int GameId { get; set; }

        public virtual Game Game { get; set; }

        public int BetId { get; set; }

        public virtual Bet Bet { get; set; }

        public virtual ResultPrediction ResultPrediction { get; set; }
    }
}