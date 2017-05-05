namespace _03.GetMinionNames
{
    using System;
    using System.Data.SqlClient;

    class GetMinionsNames
    {
        public static void GettingMinionsNames(SqlConnection connection)
        {
            int id = int.Parse(Console.ReadLine());

            string commandVillainsString = "SELECT Name FROM Villains WHERE Id = @villainId";
            SqlCommand commandVillains = new SqlCommand(commandVillainsString, connection);
            commandVillains.Parameters.AddWithValue("@villainId", id);

            using (connection)
            {
                connection.Open();
                SqlDataReader readerVillains = commandVillains.ExecuteReader();
                using (readerVillains)
                {
                    if (!readerVillains.HasRows)
                    {
                        Console.WriteLine("No villain with ID " + id + " exists in the database.");
                        return;
                    }
                    readerVillains.Read();
                    string villainName = readerVillains["Name"].ToString();
                    Console.WriteLine("Villain: " + villainName);
                }


                string commandMinionsString = "SELECT m.Name, Age\n" +
                                              "FROM Villains v\n" +
                                              "JOIN MinionsVillains mv ON v.Id = mv.VillainId\n" +
                                              "JOIN Minions m ON m.id = mv.MinionId\n" +
                                              "WHERE v.Id = @villainId";
                SqlCommand commandMinions = new SqlCommand(commandMinionsString, connection);
                commandMinions.Parameters.AddWithValue("@villainId", id);


                SqlDataReader readerMinions = commandMinions.ExecuteReader();
                using (readerMinions)
                {
                    if (!readerMinions.HasRows)
                    {
                        Console.WriteLine("(no minions)");
                        return;
                    }

                    int counter = 1;
                    while (readerMinions.Read())
                    {
                        Console.WriteLine(counter + ". " + readerMinions["Name"] + " " + readerMinions["Age"]);
                        counter++;
                    }
                }
            }
        }
    }
}
