namespace BankSystemComplex.Core.Commands
{
    using Data;
    using Data.Models;
    using Data.Validators;
    using System;
    using System.Linq;

    public class RegisterCommand
    {
        public UserValidator UserValidator { get; set; }

        public string Execute(string[] input)
        {
            if (input.Length != 3)
            {
                throw new ArgumentException("Input is not valid!");
            }

            if (AuthenticationManager.IsAuthenticated())
            {
                throw new InvalidOperationException("You should log out before log in another user!");
            }

            // Register <username> <password> <email>
            string username = input[0];
            string password = input[1];
            string email = input[2];

            User user = new User()
            {
                Username = username,
                Password = password,
                Email = email
            };

            ValidationResult valResult = this.UserValidator.IsValid(user);
            if (!valResult.IsValid)
            {
                throw new ArgumentException(string.Join("\n", valResult.ValidationErrors));
            }

            using (BankSystemContext context = new BankSystemContext())
            {
                if (context.Users.Any(u => u.Username == username))
                {
                    throw new ArgumentException("Username already taken!");
                }

                if (context.Users.Any(u => u.Email == email))
                {
                    throw new ArgumentException("Email already taken!");
                }

                context.Users.Add(user);
                context.SaveChanges();
            }

            return $"User {username} registered successfully!";
        }
    }
}