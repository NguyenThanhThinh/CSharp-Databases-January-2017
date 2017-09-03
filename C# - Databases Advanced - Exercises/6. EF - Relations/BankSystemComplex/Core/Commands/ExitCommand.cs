namespace BankSystemComplex.Core.Commands
{
    using System;

    public class ExitCommand
    {
        public string Execute(string[] input)
        {
            Environment.Exit(0);

            return string.Empty;
        }
    }
}