namespace _08.IncreaseMinionAge
{
    using System;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;

    class IncreaseMinionAge
    {
        public static void IncreaseAge(SqlConnection connection)
        {
            using (connection)
            {
                connection.Open();
                string input = Console.ReadLine();

                int[] minionsIds = input.Split(' ').Select(int.Parse).ToArray();
                StringBuilder updateMinionStringBuilder = UpdateMinionNameAndAge(minionsIds);

                SqlCommand updateMinion = new SqlCommand(updateMinionStringBuilder.ToString(), connection);
                for (int i = 0; i < minionsIds.Length; i++)
                {
                    updateMinion.Parameters.AddWithValue(@"minionId" + i, minionsIds[i]);
                }

                updateMinion.ExecuteNonQuery();

                string allMinionsString = "SELECT * FROM Minions";
                SqlCommand allMinions = new SqlCommand(allMinionsString, connection);

                SqlDataReader minionsReader = allMinions.ExecuteReader();
                using (minionsReader)
                {
                    while (minionsReader.Read())
                    {
                        for (int i = 0; i < minionsReader.FieldCount; i++)
                        {
                            Console.Write($"{minionsReader[i]} ");
                        }
                        Console.WriteLine();
                    }
                }
            }
        }

        private static StringBuilder UpdateMinionNameAndAge(int[] minionsIds)
        {
            StringBuilder updateMinionStringBuilder = new StringBuilder();
            for (int i = 0; i < minionsIds.Length; i++)
            {
                updateMinionStringBuilder.AppendLine("UPDATE Minions " +
                                                        "SET Age += 1, Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)) " +
                                                      "WHERE Id = @minionId" + i);
                updateMinionStringBuilder.AppendLine();
            }
            return updateMinionStringBuilder;
        }
    }
}
