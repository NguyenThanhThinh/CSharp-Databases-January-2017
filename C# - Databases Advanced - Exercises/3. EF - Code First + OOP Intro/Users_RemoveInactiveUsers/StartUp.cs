namespace Users_RemoveInactiveUsers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Users;

    class StartUp
    {
        static void Main(string[] args)
        {
            //If we want this task to be able to execute correct we have to 'comment' the Password and Username checks in the User Model, because
            //the data added through 'InsertUsers.sql' file wasn't validated by our Password checks and the data was added to the database
            //through the 'back door' with this sql file, instead with C# code - with C# code the checks wouldn't let the incorrect data to be added;

            //However if we want the result to be added to the database, because of the 'commented' checks the EF wants us to enable the project
            //migrations - if we just try to execute the program without using context.SaveChanges() - we can see that the correct result is given; 

            UsersContext context = new UsersContext();

            Console.WriteLine("Enter date: ");
            string enteredDateString = Console.ReadLine();
            DateTime enteredDate = DateTime.Parse(enteredDateString);
            RemoveInactiveUsers(context, enteredDate);
        }

        private static void RemoveInactiveUsers(UsersContext context, DateTime enteredDate)
        {
            List<User> users = context.Users.Where(user => user.LastTimeLoggedIn < enteredDate && !user.IsDeleted).ToList();
            foreach (User user in users)
            {
                user.IsDeleted = true;
            }
            if (users.Count == 0)
            {
                Console.WriteLine("No users have been deleted");
            }
            else
            {
                Console.WriteLine($"{users.Count} users have been deleted");
            }
        }
    }
}
