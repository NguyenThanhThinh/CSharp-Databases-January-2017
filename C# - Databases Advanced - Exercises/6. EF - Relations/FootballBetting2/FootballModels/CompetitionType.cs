namespace FootballModels
{
    using System.Collections.Generic;

    public enum Type
    {
        Local,
        National,
        International
    }

    public class CompetitionType
    {
        public CompetitionType()
        {
            this.Competitions = new HashSet<Competition>();
        }

        public int Id { get; set; }

        public Type Type { get; set; }

        public virtual ICollection<Competition> Competitions { get; set; }
    }
}
