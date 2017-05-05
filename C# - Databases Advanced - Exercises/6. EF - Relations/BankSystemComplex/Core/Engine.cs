namespace BankSystemComplex.Core
{
    using System;

    public class Engine
    {
        public CommandDispatcher CommandDispatcher { get; set; }

        public void Run()
        {
            while (true)
            {
                try
                {
                    string input = Console.ReadLine();
                    string output = this.CommandDispatcher.Dispatch(input);
                    Console.WriteLine(output);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
