namespace WeddingsPlanner.Models
{
    using System.Collections.Generic;

    public class Venue
    {
        public Venue()
        {
            this.Weddings = new HashSet<Wedding>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int Capacity { get; set; }

        public string Town { get; set; }

        public virtual ICollection<Wedding> Weddings { get; set; }
    }
}