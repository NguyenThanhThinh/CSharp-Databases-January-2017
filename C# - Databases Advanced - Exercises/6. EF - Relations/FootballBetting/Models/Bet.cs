namespace FootballBetting.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Bet
    {
        public Bet()
        {
            this.BetGames = new HashSet<BetGame>();
        }

        [Key]
        public int Id { get; set; }

        public decimal BetMoney { get; set; }

        public DateTime DateTimeOfBet { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<BetGame> BetGames { get; set; }
    }
}