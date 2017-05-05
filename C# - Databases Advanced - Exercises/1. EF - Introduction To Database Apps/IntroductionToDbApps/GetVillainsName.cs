namespace _02.GetVillainsNames
{
    using System;
    using System.Data.SqlClient;

    class GetVillainsNames
    {
        public static void GettingVillainsNames(SqlConnection connection)
        {
            string commandString = "SELECT v.Name, COUNT(MinionId) AS Count\n" +
                                        "FROM Villains AS v\n" +
                                        "JOIN MinionsVillains AS mv ON v.Id = mv.VillainId\n" +
                                       "GROUP BY v.Name\n" +
                                      "HAVING COUNT(MinionId) > 3\n" +
                                       "ORDER BY Count DESC";
            SqlCommand command = new SqlCommand(commandString, connection);

            connection.Open();

            using (connection)
            {
                SqlDataReader reader = command.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(reader["Name"] + " " + reader["Count"]);
                    }
                }
            }
        }
    }
}
