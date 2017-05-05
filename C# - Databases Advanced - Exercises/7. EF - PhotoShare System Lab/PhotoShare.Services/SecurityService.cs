namespace PhotoShare.Services
{
    using Data;
    using Models;
    using System;
    using System.Linq;

    public class SecurityService
    {
        private static User loggedUser;

        public static void Login(string username, string password)
        {
            using (PhotoShareContext context = new PhotoShareContext())
            {
                User user = context.Users.SingleOrDefault(u => u.Username == username && u.Password == password);

                if (loggedUser != null)
                {
                    throw new ArgumentException("You should logout first!");
                }

                if (user == null)
                {
                    throw new ArgumentException("Invalid username or password!");
                }

                loggedUser = user;
            }
        }

        public static void Logout()
        {
            if (loggedUser == null)
            {
                throw new InvalidOperationException("You should login first!");
            }

            loggedUser = null;
        }

        public static User GetCurrentUser()
        {
            return loggedUser;
        }

        public static bool IsAuthenticated()
        {
            return loggedUser != null;
        }
    }
}

