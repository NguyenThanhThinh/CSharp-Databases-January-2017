namespace TeamBuilder.App.Core.Commands
{
    using Data;
    using Models;
    using System;
    using System.Linq;
    using Utilities;

    public class RegisterUserCommand
    {
        // RegisterUser <username> <password> <repeat-password> <firstName> <lastName> <age> <gender>
        public string Execute(string[] inputArgs)
        {
            // Validate input length
            Check.CheckLength(7, inputArgs);

            // Validate username
            string username = inputArgs[0];
            if (username.Length < Constants.MinUsernameLength || username.Length > Constants.MaxUsernameLength)
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.UsernameNotValid, username));
            }

            // Validate password
            string password = inputArgs[1];
            if (!password.Any(char.IsDigit) || !password.Any(char.IsUpper))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.PasswordNotValid, password));
            }

            // Validate repeated password
            string repeatedPassword = inputArgs[2];
            if (password != repeatedPassword)
            {
                throw new InvalidOperationException(string.Format(Constants.ErrorMessages.PasswordsDoNotMatch));
            }

            // Validate first name
            string firstName = inputArgs[3];
            if (firstName.Length > Constants.MaxFirstNameLength)
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.FirstNameNotValid));
            }

            // Validate second name
            string lastName = inputArgs[4];
            if (lastName.Length > Constants.MaxLastNameLength)
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.LastNameNotValid));
            }

            // Validate age
            int age;
            bool isNumber = int.TryParse(inputArgs[5], out age);

            if (!isNumber || age <= 0)
            {
                throw new ArgumentException(Constants.ErrorMessages.AgeNotValid);
            }

            // Validate gender
            Gender gender;
            bool isGenderValid = Enum.TryParse(inputArgs[6], out gender);

            if (!isGenderValid)
            {
                throw new ArgumentException(Constants.ErrorMessages.GenderNotValid);
            }

            // Validate if user is already existing
            if (CommandHelper.IsUserExisting(username))
            {
                throw new InvalidOperationException(string.Format(Constants.ErrorMessages.UsernameIsAlreadyTaken, username));
            }

            this.RegisterUser(username, password, firstName, lastName, age, gender);

            return $"User {username} was registered successfully!";
        }

        private void RegisterUser(string username, string password, string firstName, string lastName, int age, Gender gender)
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                User user = new User()
                {
                    Username = username,
                    Password = password,
                    FirstName = firstName,
                    LastName = lastName,
                    Age = age,
                    Gender = gender
                };

                context.Users.Add(user);
                context.SaveChanges();
            }
        }
    }
}