namespace _06.RemoveVillain
{
    using System;
    using System.Data.SqlClient;

    internal class RemoveVillain
    {
        public static void RemoveVillainFromDatabase(SqlConnection connection)
        {
            int villainId = int.Parse(Console.ReadLine());
            using (connection)
            {
                connection.Open();
                SqlTransaction delete = connection.BeginTransaction();

                string villainString = "SELECT Id, Name FROM villains WHERE Id = @villainId";
                SqlCommand villain = new SqlCommand(villainString, connection);
                villain.Parameters.AddWithValue("@villainId", villainId);

                string deleteVillainString = "DELETE FROM Villains WHERE Id = @villainId";
                SqlCommand deleteVillain = new SqlCommand(deleteVillainString, connection);
                deleteVillain.Parameters.AddWithValue("@villainId", villainId);

                string releaseMinionsString = "DELETE FROM MinionsVillains WHERE VillainId = @villainId";
                SqlCommand releaseMinions = new SqlCommand(releaseMinionsString, connection);
                releaseMinions.Parameters.AddWithValue("@villainId", villainId);

                villain.Transaction = delete;
                SqlDataReader reader = villain.ExecuteReader();
                try
                {
                    reader.Read();
                    string villainName = reader["Name"].ToString();
                    reader.Close();

                    releaseMinions.Transaction = delete;
                    int releasedMinions = releaseMinions.ExecuteNonQuery();

                    deleteVillain.Transaction = delete;
                    deleteVillain.ExecuteNonQuery();

                    delete.Commit();
                    Console.WriteLine("{0} was deleted", villainName);
                    Console.WriteLine("{0} minions were released", releasedMinions);
                }
                catch (InvalidOperationException e)
                {
                    reader.Close();
                    delete.Rollback();
                    Console.WriteLine("No such villain was found");
                }
            }
        }
    }
}