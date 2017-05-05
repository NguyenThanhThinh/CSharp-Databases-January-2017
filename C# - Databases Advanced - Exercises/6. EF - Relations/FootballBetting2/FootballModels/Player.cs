namespace FootballModels
{
    using System.Collections.Generic;

    public class Player
    {
        public Player()
        {
            this.PlayerStatistics = new HashSet<PlayerStatistic>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int SquadNumber { get; set; }

        public int TeamId { get; set; }

        public virtual Team Team { get; set; }

        public int PositionId { get; set; }

        public virtual Position Position { get; set; }

        public bool IsInjured { get; set; }

        public virtual ICollection<PlayerStatistic> PlayerStatistics { get; set; }
    }
}
