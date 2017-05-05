namespace TeamBuilder.Models
{
    using Validations;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public enum Gender
    {
        Male,
        Female
    };

    public class User
    {
        public User()
        {
            this.Teams = new HashSet<Team>();
            this.CreatedTeams = new HashSet<Team>();   
            this.CreatedEvents = new HashSet<Event>();
            this.ReceivedInvitations = new HashSet<Invitation>(); 
        }

        public int Id { get; set; }

        [MinLength(3)]
        public string Username { get; set; }

        [MinLength(6)]
        [Password(ContainsDigit = true, ContainsUppercase = true, ErrorMessage = "Invalid password")]
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public Gender Gender { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<Team> Teams { get; set; }

        public virtual ICollection<Team> CreatedTeams { get; set; }

        public virtual ICollection<Event> CreatedEvents { get; set; }

        public virtual ICollection<Invitation> ReceivedInvitations{ get; set; }
    }
}
