namespace Users_GetUsersByEmailProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Users;

    internal class StartUp
    {
        private static void Main(string[] args)
        {
            //If we want this task to be able to execute correct we have to 'comment' the Password checks in the User Model, because
            //the data added through 'InsertUsers.sql' file wasn't validated by our Password checks and the data was added to the database
            //through the 'back door' with this sql file, instead with C# code - with C# code the checks wouldn't let the incorrect data to be added;

            UsersContext context = new UsersContext();

            Console.WriteLine("Enter email provider: ");
            string emailProvider = Console.ReadLine();
            GetUsersByEmailProvider(context, emailProvider);
        }

        private static void GetUsersByEmailProvider(UsersContext context, string emailProvider)
        {
            IEnumerable<User> usersByEmailProvider = context.Users.Where(x => x.Email.EndsWith(emailProvider));

            foreach (var user in usersByEmailProvider)
            {
                Console.WriteLine($"{user.Username} {user.Email}");
            }
        }
    }
}