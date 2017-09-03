namespace PhotoShare.Client.Core.Commands
{
    using Services;
    using System;

    public class ExitCommand
    {
        public void Execute()
        {
            if (SecurityService.IsAuthenticated())
            {
                SecurityService.Logout();
            }

            Console.WriteLine("Good Bye!");
            Environment.Exit(0);
        }
    }
}