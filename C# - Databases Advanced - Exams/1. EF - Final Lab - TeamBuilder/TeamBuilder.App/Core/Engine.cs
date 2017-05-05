namespace TeamBuilder.App.Core
{
    using System;

    public class Engine
    {
        private CommandDispatcher commandDispatcher;

        public Engine(CommandDispatcher commandDispatcher)
        {
            this.commandDispatcher = commandDispatcher;
        }

        public void Run()
        {
            while (true)
            {
                try
                {
                    string input = Console.ReadLine();
                    string result = this.commandDispatcher.Dispatch(input);
                    Console.WriteLine(result);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.GetBaseException().Message);
                }
            }
        }
    }
}
