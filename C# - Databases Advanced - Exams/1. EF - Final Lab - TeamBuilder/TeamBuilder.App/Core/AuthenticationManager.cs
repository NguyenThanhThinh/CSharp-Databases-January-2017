namespace TeamBuilder.App.Core
{
    using Models;
    using System;
    using Utilities;

    public class AuthenticationManager
    {
        private static User loggedUser;

        public static void Login(User user)
        {
            if (loggedUser != null)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.LogoutFirst);
            }

            loggedUser = user;
        }

        public static void Logout()
        {
            if (loggedUser == null)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.LoginFirst);
            }

            loggedUser = null;
        }

        public static User GetCurrentUser()
        {
            if (loggedUser == null)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.LoginFirst);
            }

            return loggedUser;
        }

        public static void Authorize()
        {
            if (loggedUser == null)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.LoginFirst);
            }
        }

        public static void HasAlreadyLoggedUser()
        {
            if (loggedUser != null)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.LogoutFirst);
            }
        }
    }
}