namespace Users
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class User
    {
        private string email;

        private string password;

        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string Username { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(50)]
        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                //These checks have to be commented in order to work Problem 11 and 12;

                //if(!this.CheckIfLowerLetterIsContained(value)
                //   || !this.CheckIfUpperLetterIsContained(value)
                //   || !this.CheckIfDigitIsContained(value)
                //   || !this.CheckIfSpecialSymbolIsContained(value))
                //{
                //    throw new ArgumentException("The password must contain at least one lower letter, one capital letter, one digit and one special symbol!");
                //}

                this.password = value;
            }
        }

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

        [MaxLength(1024 * 1024)]
        public byte[] ProfilePicture { get; set; }

        public DateTime RegisteredOn { get; set; }

        public DateTime LastTimeLoggedIn { get; set; }

        [Range(1, 120)]
        public int Age { get; set; }

        public bool IsDeleted { get; set; }
    }
}