namespace WeddingsPlanner.Models
{
    using Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Person
    {
        public Person()
        {
            this.BrideWeddings = new HashSet<Wedding>();
            this.BridegroomWeddings = new HashSet<Wedding>();
            this.Invitations = new HashSet<Invitation>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(60)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(1)]
        public string MiddleNameInitial { get; set; }

        [Required]
        [MinLength(2)]
        public string LastName { get; set; }

        [NotMapped]
        public string FullName => $"{this.FirstName} {this.MiddleNameInitial} {this.LastName}";

        [Required]
        public Gender Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        [NotMapped]
        public int? Age
        {
            get
            {
                if (BirthDate == null)
                {
                    return null;
                }

                DateTime now = DateTime.Now;
                int age = now.Year - this.BirthDate.Value.Year;

                if (now.Month < BirthDate.Value.Month ||
                    (now.Month == BirthDate.Value.Month && now.Day < BirthDate.Value.Day))
                {
                    age--;
                }

                return age;
            }
        }

        public string Phone { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9]+@[a-z]{1,}.[a-z]{1,}$")]
        public string Email { get; set; }

        [InverseProperty("Bride")]
        public virtual ICollection<Wedding> BrideWeddings { get; set; }

        [InverseProperty("Bridegroom")]
        public virtual ICollection<Wedding> BridegroomWeddings { get; set; }

        public virtual ICollection<Invitation> Invitations { get; set; }
    }
}
