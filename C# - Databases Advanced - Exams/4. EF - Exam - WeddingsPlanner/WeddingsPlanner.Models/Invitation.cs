namespace WeddingsPlanner.Models
{
    using Enums;
    using System.ComponentModel.DataAnnotations;

    public class Invitation
    {
        public int Id { get; set; }

        public int WeddingId { get; set; }

        public virtual Wedding Wedding { get; set; }

        public int GuestId { get; set; }

        public virtual Person Guest { get; set; }

        public int PresentId { get; set; }

        public virtual Present Present { get; set; }

        public bool IsAttending { get; set; }

        [Required]
        public Family Family { get; set; }
    }
}