namespace _05.ChangeTownNamesCasing
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Text;

    internal class ChangeTownNamesCasing
    {
        public static void ChangeTownNames(SqlConnection connection)
        {
            string inputCountry = Console.ReadLine();
            List<string> townNames = new List<string>();

            string commandStringTownNames = "SELECT Name " +
                                              "FROM Towns " +
                                             "WHERE Country = @country";
            SqlCommand commandTownNames = new SqlCommand(commandStringTownNames, connection);
            commandTownNames.Parameters.AddWithValue("@country", inputCountry);

            using (connection)
            {
                connection.Open();

                SqlDataReader reader = commandTownNames.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        townNames.Add(reader["Name"].ToString());
                    }
                }
            }

            for (int currTown = 0; currTown < townNames.Count; currTown++)
            {
                townNames[currTown] = townNames[currTown].ToUpper();
            }

            StringBuilder result = new StringBuilder();
            if (townNames.Count != 0)
            {
                result.AppendLine($"{townNames.Count} towns were affected.");
                result.AppendLine($"[{String.Join(", ", townNames)}]");
            }
            else
            {
                result.AppendLine("No town names were affected.");
            }
            Console.Write(result.ToString());
        }
    }
}