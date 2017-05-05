namespace PhotoShare.Client.Core.Commands
{
    using System;
    using Services;

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
