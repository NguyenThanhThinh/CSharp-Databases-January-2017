namespace PhotoShare.Client.Core.Commands
{
    using Models;
    using System;
    using Services;
    using System.Linq;

    public class ModifyUserCommand
    {
        private UserService userService;
        private TownService townService;

        public ModifyUserCommand(UserService userService, TownService townService)
        {
            this.userService = userService;
            this.townService = townService;
        }

        // ModifyUser <username> <property> <new value>
        public string Execute(string[] data)
        {
            string username = data[0];
            string propertyType = data[1];
            string propertyNewValue = data[2];

            if (!SecurityService.IsAuthenticated())
            {
                throw new InvalidOperationException("Login in order to modify your profile!");
            }

            if (!this.userService.IsUserExisting(username))
            {
                throw new ArgumentException($"User {username} not found!");
            }

            User userToUpdate = this.userService.GetUserByUsername(username);

            if (SecurityService.GetCurrentUser().Username != username)
            {
                throw new InvalidOperationException("You can only modify your own profile!");
            }

            if (propertyType == "Password")
            {
                if (!(propertyNewValue.Any(char.IsLower) && propertyNewValue.Any(char.IsDigit)))
                {
                    throw new ArgumentException($"Value {propertyNewValue} not valid!\nInvalid Password!");
                }

                userToUpdate.Password = propertyNewValue;
            }
            else if (propertyType == "BornTown")
            {
                if (!this.townService.IsTownExisting(propertyNewValue))
                {
                    throw new ArgumentException($"Value {propertyNewValue} not valid!\nTown {propertyNewValue} not found!");
                }

                userToUpdate.BornTownId = this.townService.GetTownByTownName(propertyNewValue).Id;
            }
            else if (propertyType == "CurrentTown")
            {
                if (!this.townService.IsTownExisting(propertyNewValue))
                {
                    throw new ArgumentException($"Value {propertyNewValue} not valid!\nTown {propertyNewValue} not found!");
                }

                userToUpdate.CurrentTownId = this.townService.GetTownByTownName(propertyNewValue).Id;
            }
            else
            {
                throw new ArgumentException($"Property {propertyType} not supported!");
            }

            this.userService.UpdateUser(userToUpdate);

            return $"User {username} {propertyType} is {propertyNewValue}!";
        }
    }
}
