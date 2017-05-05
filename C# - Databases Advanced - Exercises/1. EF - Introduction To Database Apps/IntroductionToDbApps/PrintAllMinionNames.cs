namespace _07.PrintAllMinionNames
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    class PrintAllMinionNames
    {
        public static void MinionNames(SqlConnection connection)
        {
            using (connection)
            {
                connection.Open();
                List<string> minionNames = new List<string>();

                string allMinionsString = "SELECT Name FROM Minions";
                SqlCommand allMinions = new SqlCommand(allMinionsString, connection);

                SqlDataReader reader = allMinions.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        minionNames.Add(reader["Name"].ToString());
                    }
                }

                PrintMinionNames(minionNames);
            }
        }

        static void PrintMinionNames(List<string> minionNames)
        {
            List<string> namesToPrint = new List<string>();
            int counter = 0;

            for (int currMinion = 0; currMinion < minionNames.Count; currMinion++)
            {
                if (currMinion % 2 == 0)
                {
                    namesToPrint.Add(minionNames[counter]);
                }
                else
                {
                    namesToPrint.Add(minionNames[minionNames.Count - 1 - counter]);
                    counter++;
                }
            }
            Console.WriteLine(string.Join("\n", namesToPrint));
        }
    }
}
