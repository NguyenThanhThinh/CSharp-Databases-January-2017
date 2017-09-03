namespace FootballModels
{
    using System;
    using System.Collections.Generic;

    public class Bet
    {
        public Bet()
        {
            this.BetGames = new HashSet<BetGame>();
        }

        public int Id { get; set; }

        public decimal BetMoney { get; set; }

        public DateTime DateTime { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<BetGame> BetGames { get; set; }
    }
}