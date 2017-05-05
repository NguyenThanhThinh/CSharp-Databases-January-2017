using _02.GetVillainsNames;
using _03.GetMinionNames;
using _04.AddMinion2;
using _05.ChangeTownNamesCasing;
using _06.RemoveVillain;
using _07.PrintAllMinionNames;
using _08.IncreaseMinionAge;
using _09.IncreaseAgeStoredProcedure;

namespace IntroductionToDbApps
{
    using System.Data.SqlClient;
    using System.IO;

    class _IntroductionToDbApps
    {
        static void Main(string[] args)
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
