namespace PhotoShare.Client.Core.Commands
{
    using Models;
    using System;
    using Services;

    class LogoutUserCommand
    {
        public string Execute(string[] data)
        {
            User user = SecurityService.GetCurrentUser();

            if (user == null)
            {
                throw new InvalidOperationException("You should log in first in order to logout.");
            }

            SecurityService.Logout();

            return $"User {user.Username} successfully logged out!";
        }
    }
}
