namespace Stations.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public enum CardType
    {
        Pupil,
        Student,
        Elder,
        Debilitated,
        Normal
    }

    public class CustomerCard
    {
        public CustomerCard()
        {
            this.BoughtTickets = new HashSet<Ticket>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(128)]
        public string Name { get; set; }

        [Range(0, 120)]
        public int Age { get; set; }

        public CardType Type { get; set; } 

        public virtual ICollection<Ticket> BoughtTickets { get; set; }
    }
}
