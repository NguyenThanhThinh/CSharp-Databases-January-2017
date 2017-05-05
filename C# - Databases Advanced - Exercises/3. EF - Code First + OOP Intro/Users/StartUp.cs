namespace Users
{
    using System.Data.Entity.Validation;
    using System;

    class StartUp
    {
        static void Main(string[] args)
        {
            //The public partial class User is defined in folder Models;
            //The other part of the partial class - the helper methods for checking the correct user information - is in folder ModelsExtensions;

            //If we don't have the database created - we have to initialize it first;
            //Then we can insert all the users in it - by executing the InsertUsers.sql file;

            //However by inserting the file using SQL commands we actually skip the check methods which we have created:
            //The check methods are working and catch exceptions only if we add a new user by EF and the console! 
            //You can check the validations below:

            var context = new UsersContext();
            //context.Database.Initialize(true);

            User user = new User()
            {
                Username = "4-30 symbols", //required

                Password = "6-50 symbols - lower - UPPER - [0-9] - [!?@#$%^&*]", //required

                Email = "britain_email@domain.co.uk", //"required - <user>@<host> - symbols '.'|'-'|'_' aren't possible in the beggining/ending of the words"

                ProfilePicture = null, //"size of maximum 1MB or null"

                RegisteredOn = new DateTime(2017, 03, 01),

                LastTimeLoggedIn = new DateTime(2017, 03, 03),

                Age = 80, //"range between 1-120"

                IsDeleted = false
            };

            context.Users.Add(user);

            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var ev in ex.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        ev.Entry.Entity.GetType().Name, ev.Entry.State);
                    foreach (var ve in ev.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

                //string connectionString = "data source=(localdb)\\mssqllocaldb;initial catalog=userscontext;integrated security=true";
                //SqlConnection connection = new SqlConnection(connectionString);

                //string insertUsersString = File.ReadAllText("../../insertusers.sql");
                //SqlCommand insertUsers = new SqlCommand(insertUsersString, connection);

                //connection.Open();
                //using (connection)
                //{
                //    insertUsers.ExecuteNonQuery();
                //    Console.WriteLine($"number of inserted users: {context.Users.Count()}");
                //}
            }
    }
}
