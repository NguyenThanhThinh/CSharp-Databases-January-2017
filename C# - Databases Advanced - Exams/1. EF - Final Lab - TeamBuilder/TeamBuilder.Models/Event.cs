namespace TeamBuilder.Models
{
    using System;
    using System.Collections.Generic;

    public class Event
    {
        private DateTime endDate;

        private DateTime startDate;

        public Event()
        {
            this.ParticipatingTeams = new HashSet<Team>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate
        {
            get { return this.startDate; }
            set { this.startDate = value; }
        }

        public DateTime EndDate
        {
            get { return this.endDate; }
            set
            {
                if (startDate > value)
                {
                    throw new ArgumentException("The start date cannot be later than the end date!");
                }

                this.endDate = value;
            }
        }

        public int CreatorId { get; set; }

        public virtual User Creator { get; set; }

        public virtual ICollection<Team> ParticipatingTeams { get; set; }
    }
}