namespace TeamBuilder.App.Core.Commands
{
    using System;
    using Utilities;

    public class ExitCommand
    {
        public void Execute(string[] inputArgs)
        {
            Check.CheckLength(0, inputArgs);

            Console.WriteLine("Good bye!");

            Environment.Exit(0);
        }
    }
}