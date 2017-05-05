namespace BankSystem
{
    using BankSystem.Data;
    using System;
    using Core;

    //Solution added from Georgi on the website;
    public class StartUp
    {
        public static void Main(string[] args)
        {
            CommandExecutor executor = new CommandExecutor();
            //BankSystemContext context = new BankSystemContext();
            //context.Database.Initialize(true);

            while (true)
            {
                try
                {
                    string input = Console.ReadLine();
                    string output = executor.Execute(input);
                    Console.WriteLine(output);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
