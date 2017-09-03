namespace BankSystemComplex.Core
{
    using Data.Models;
    using System;

    // Class used for simple authentication validation.
    // NOTE: If class is declared as static all of its members, fields and methods MUST be static as well.
    public static class AuthenticationManager
    {
        private static User currentUser;

        // Checks if there is current logged in user.
        public static bool IsAuthenticated()
        {
            return currentUser != null;
        }

        // Logout current user.
        public static void Logout()
        {
            if (!IsAuthenticated())
            {
                throw new InvalidOperationException("You should login first!");
            }

            currentUser = null;
        }

        // Logs in the user specified.
        public static void Login(User user)
        {
            if (IsAuthenticated())
            {
                throw new InvalidOperationException("You should logout first!");
            }

            if (user == null)
            {
                throw new InvalidOperationException("User to log in is invalid!");
            }

            currentUser = user;
        }

        // Gets currently logged in user.
        public static User GetCurrentUser()
        {
            return currentUser;
        }
    }
}