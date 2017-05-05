namespace Hospital.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;

    public class Patient
    {
        private string email;

        public Patient()
        {
            this.Visitations = new HashSet<Visitation>();
            this.Diagnoses = new HashSet<Diagnose>();
            this.Medicaments = new HashSet<Medicament>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Email
        {
            get
            {
                return this.email;
            }
            set
            {
                if (!CheckIfEmailIsValid(value))
                {
                    throw new ArgumentException("The email is not in the correct format!");
                }

                this.email = value;
            }
        }

        public DateTime DateOfBirth { get; set; }

        public byte[] Picture { get; set; }

        public bool HasMedicalInsuranse { get; set; }

        public ICollection<Visitation> Visitations { get; set; }

        public ICollection<Diagnose> Diagnoses { get; set; }

        public ICollection<Medicament> Medicaments { get; set; }

        private bool CheckIfEmailIsValid(string email)
        {
            string regularExpressionString = @"([a-zA-Z0-9][a-zA-Z_\-.]*[a-zA-Z0-9])@([a-zA-Z]+\.[a-zA-Z]+(\.[a-zA-Z-]+)*)\b";
            Regex regex = new Regex(regularExpressionString);

            if (!regex.IsMatch(email))
            {
                return false;
            }

            return true;
        }
    }
}
