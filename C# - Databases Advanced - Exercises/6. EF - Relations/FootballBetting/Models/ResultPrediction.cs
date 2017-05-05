namespace FootballBetting.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum Prediction
    {
        HomeTeamWin,
        DrawGame,
        AwatTeamWin
    }

    public class ResultPrediction
    {
        [Key]
        public int Id { get; set; }

        public virtual Prediction Prediction { get; set; }
    }
}
