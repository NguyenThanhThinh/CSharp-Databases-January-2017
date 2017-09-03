namespace FootballModels
{
    public enum Predictions
    {
        HomeTeamWin,
        AwayTeamWin,
        DrawGame
    }

    public class ResultPrediction
    {
        public int Id { get; set; }

        public virtual Predictions Prediction { get; set; }

        public virtual BetGame BetGame { get; set; }
    }
}