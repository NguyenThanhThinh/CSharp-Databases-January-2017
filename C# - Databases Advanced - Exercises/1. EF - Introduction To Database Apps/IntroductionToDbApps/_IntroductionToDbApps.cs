namespace IntroductionToDbApps
{
    using System.Data.SqlClient;

    internal class _IntroductionToDbApps
    {
        private static void Main(string[] args)
        {
            //string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Integrated Security=True";

            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MinionsDB;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);

            //Each problem is solved in different class - uncomment the class to start executing the solution for given problem with CTRL+F5:
            //After that comment again the class and uncomment another one to execute it;

            //InitialSetup.CreateDatabaseAndTables(connection);
            //GetVillainsNames.GettingVillainsNames(connection);
            //GetMinionsNames.GettingMinionsNames(connection);
            //AddMinion.AddingMinion(connection);
            //ChangeTownNamesCasing.ChangeTownNames(connection);
            //RemoveVillain.RemoveVillainFromDatabase(connection);
            //PrintAllMinionNames.MinionNames(connection);
            //IncreaseMinionAge.IncreaseAge(connection);
            //IncreaseAgeStoredProcedure.IncreaseAge(connection);
        }
    }
}