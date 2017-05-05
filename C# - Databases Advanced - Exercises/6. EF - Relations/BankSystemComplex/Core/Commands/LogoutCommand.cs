namespace BankSystemComplex.Core.Commands
{
    using System;
    using Data.Models;

    public class LogoutCommand
    {
        public string Execute(string[] input)
        {
            if (!AuthenticationManager.IsAuthenticated())
            {
                throw new InvalidOperationException("You should log in first!");
            }

            User user = AuthenticationManager.GetCurrentUser();
            AuthenticationManager.Logout();

            return $"User {user.Username} successfully logged out!";
        }
    }
}
