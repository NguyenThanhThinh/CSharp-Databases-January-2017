namespace FootballModels
{
    using System.Collections.Generic;

    public class Position
    {
        public Position()
        {
            this.Players = new HashSet<Player>();
        }

        public int Id { get; set; }

        public string PositionDescription { get; set; }

        public virtual ICollection<Player> Players { get; set; }
    }
}
