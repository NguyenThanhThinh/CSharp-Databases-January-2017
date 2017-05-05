namespace WeddingsPlanner.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Present
    {
        [Key]
        public int InvitationId { get; set; }

        public virtual Invitation Invitation { get; set; }
        
        [NotMapped]
        public Person Owner => this.Invitation.Guest;
    }
}
