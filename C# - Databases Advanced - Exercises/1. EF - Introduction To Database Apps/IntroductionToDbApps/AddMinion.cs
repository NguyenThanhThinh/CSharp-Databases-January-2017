namespace _04.AddMinion2
{
    using System;
    using System.Data.SqlClient;

    class AddMinion
    {
        public static void AddingMinion(SqlConnection connection)
        {
            string minionInput = Console.ReadLine();
            string villianInput = Console.ReadLine();

            string[] minionData = minionInput.Split(':')[1].Trim().Split(' ');
            string minionName = minionData[0];
            int minionAge = int.Parse(minionData[1]);
            string townName = minionData[2];

            string villianName = villianInput.Split(':')[1].Trim();

            using (connection)
            {
                connection.Open();

                if (!CheckIfTownExists(townName, connection))
                {
                    AddTown(townName, connection);
                    Console.WriteLine($"Town {townName} was added to the database.");
                }

                if (!CheckIfVillianExists(villianName, connection))
                {
                    AddVillian(villianName, connection);
                    Console.WriteLine($"Villain {villianName} was added to the database.");
                }

                int townId = GetTownIdByName(townName, connection);
                AddMinionToDatabase(minionName, minionAge, townId, connection);

                int villianId = GetVillianIdByName(villianName, connection);
                int minionId = GetMinionIdByName(minionName, connection);
                AddMinionToVillian(minionId, villianId, connection);

                Console.WriteLine($"Successfully added {minionName} to be minion of {villianName}");
            }
        }

        private static bool CheckIfTownExists(string townName, SqlConnection connection)
        {
            string commandString = "SELECT COUNT(*) FROM Towns Where Name = @townName";
            SqlCommand command = new SqlCommand(commandString, connection);
            command.Parameters.AddWithValue("@townName", townName);

            if ((int)command.ExecuteScalar() == 0)
            {
                return false;
            }
            return true;
        }

        private static void AddTown(string townName, SqlConnection connection)
        {
            string commandString = "INSERT INTO Towns(Name) VALUES (@townName)";
            SqlCommand command = new SqlCommand(commandString, connection);
            command.Parameters.AddWithValue("@townName", townName);

            command.ExecuteNonQuery();
        }

        private static bool CheckIfVillianExists(string villianName, SqlConnection connection)
        {
            string commandString = "SELECT COUNT(*) FROM Villains Where Name = @villianName";
            SqlCommand command = new SqlCommand(commandString, connection);
            command.Parameters.AddWithValue("@villianName", villianName);

            if ((int)command.ExecuteScalar() == 0)
            {
                return false;
            }
            return true;
        }

        private static void AddVillian(string villianName, SqlConnection connection)
        {
            string commandString = "INSERT INTO Villains(Name, EvilnessFactor) VALUES (@villianName, 'evil')";
            SqlCommand command = new SqlCommand(commandString, connection);
            command.Parameters.AddWithValue("@villianName", villianName);

            command.ExecuteNonQuery();
        }

        private static int GetTownIdByName(string townName, SqlConnection connection)
        {
            int townId = 0;

            string commandString = "SELECT Id FROM Towns Where Name = @townName";
            SqlCommand command = new SqlCommand(commandString, connection);
            command.Parameters.AddWithValue("@townName", townName);
            townId = (int)command.ExecuteScalar();

            return townId;
        }

        private static void AddMinionToDatabase(string minionName, int minionAge, int townId, SqlConnection connection)
        {
            string commandString = "INSERT INTO Minions(Name, Age, TownId) VALUES(@minionName, @minionAge, @townId)";
            SqlCommand command = new SqlCommand(commandString, connection);
            command.Parameters.AddWithValue("@minionName", minionName);
            command.Parameters.AddWithValue("@minionAge", minionAge);
            command.Parameters.AddWithValue("@townId", townId);
            command.ExecuteNonQuery();
        }

        private static int GetVillianIdByName(string villianName, SqlConnection connection)
        {
            int villianId = 0;

            string commandString = "SELECT Id FROM Villains Where Name = @villianName";
            SqlCommand command = new SqlCommand(commandString, connection);
            command.Parameters.AddWithValue("@villianName", villianName);
            villianId = (int)command.ExecuteScalar();

            return villianId;
        }

        private static int GetMinionIdByName(string minionName, SqlConnection connection)
        {
            int minionId = 0;

            string commandString = "SELECT Id FROM Minions Where Name = @minionName";
            SqlCommand command = new SqlCommand(commandString, connection);
            command.Parameters.AddWithValue("@minionName", minionName);
            minionId = (int)command.ExecuteScalar();

            return minionId;
        }
        
        private static void AddMinionToVillian(int minionId, int villianId, SqlConnection connection)
        {
            string commandString = "INSERT INTO MinionsVillains VALUES(@minionId, @villianId)";
            SqlCommand command = new SqlCommand(commandString, connection);
            command.Parameters.AddWithValue("@minionId", minionId);
            command.Parameters.AddWithValue("@villianId", villianId);
            command.ExecuteNonQuery();
        }
    }
}
