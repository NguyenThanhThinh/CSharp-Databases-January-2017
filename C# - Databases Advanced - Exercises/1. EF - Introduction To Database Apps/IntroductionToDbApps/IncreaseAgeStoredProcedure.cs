namespace _09.IncreaseAgeStoredProcedure
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    //Created procedure in MSSQL Server Management Studio
    //CREATE PROCEDURE usp_GetOlder(@minionId INT)
    //AS
    //BEGIN
    //    UPDATE Minions
    //       SET Age += 1
    //	   WHERE Id = @minionId
    //END

    class IncreaseAgeStoredProcedure
    {
        public static void IncreaseAge(SqlConnection connection)
        {
            int minionId = int.Parse(Console.ReadLine());

            using (connection)
            {
                connection.Open();

                string getOlderProcedureString = "usp_GetOlder";
                SqlCommand getOlderProcedute = new SqlCommand(getOlderProcedureString, connection);
                getOlderProcedute.CommandType = CommandType.StoredProcedure;
                getOlderProcedute.Parameters.AddWithValue("@minionId", minionId);

                getOlderProcedute.ExecuteNonQuery();

                string minionString = "SELECT Name, Age FROM Minions WHERE Id = @minionId";
                SqlCommand minion = new SqlCommand(minionString, connection);
                minion.Parameters.AddWithValue("@minionId", minionId);

                SqlDataReader reader = minion.ExecuteReader();
                using (reader)
                {
                    reader.Read();
                    Console.WriteLine(reader["Name"] + " " + reader["Age"]);
                }
            }
        }
    }
}
