namespace FootballBetting.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PlayerStatistic
    {
        [Key]
        [Column(Order = 0)]
        [ForeignKey("Game")]
        public int GameId { get; set; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey("Player")]
        public int PlayerId { get; set; }

        public int ScoredGoals { get; set; }

        public int PlayerAssists { get; set; }

        public int PlayedMinutesDuringGame { get; set; }

        public virtual Game Game { get; set; }

        public virtual Player Player { get; set; }
    }
}
