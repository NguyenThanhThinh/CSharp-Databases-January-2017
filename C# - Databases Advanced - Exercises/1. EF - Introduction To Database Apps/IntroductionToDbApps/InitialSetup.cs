namespace IntroductionToDbApps
{
    using System.Data.SqlClient;
    using System.IO;

    internal class InitialSetup
    {
        public static void CreateDatabaseAndTables(SqlConnection connection)
        {
            connection.Open();
            string createDatabaseString = "CREATE DATABASE MinionsDB";
            SqlCommand createDatabase = new SqlCommand(createDatabaseString, connection);

            string createTablesString = File.ReadAllText("../../InitialSetup.sql");
            SqlCommand createTables = new SqlCommand(createTablesString, connection);
            using (connection)
            {
                //createDatabase.ExecuteNonQuery();
                createTables.ExecuteNonQuery();
            }
        }
    }
}