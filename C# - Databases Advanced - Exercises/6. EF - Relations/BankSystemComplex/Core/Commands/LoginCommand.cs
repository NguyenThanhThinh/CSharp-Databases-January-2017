namespace BankSystemComplex.Core.Commands
{
    using Data;
    using Data.Models;
    using System;
    using System.Linq;

    // Logs user (if found) in our application.
    public class LoginCommand
    {
        public string Execute(string[] input)
        {
            if (input.Length != 2)
            {
                throw new ArgumentException("Input is not valid!");
            }

            if (AuthenticationManager.IsAuthenticated())
            {
                throw new InvalidOperationException("You should logout first!");
            }

            // Login <username> <password>
            string username = input[0];
            string password = input[1];

            using (BankSystemContext context = new BankSystemContext())
            {
                User user = context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

                if (user == null)
                {
                    throw new ArgumentException("Invalid username/password!");
                }

                AuthenticationManager.Login(user);
            }

            return $"User {username} logged in successfully!";
        }
    }
}