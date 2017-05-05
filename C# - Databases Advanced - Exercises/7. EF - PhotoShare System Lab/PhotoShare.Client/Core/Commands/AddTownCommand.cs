namespace PhotoShare.Client.Core.Commands
{
    using System;
    using Services;

    public class AddTownCommand
    {
        private readonly TownService townService;

        public AddTownCommand(TownService townService)
        {
            this.townService = townService;
        }

        // AddTown <townName> <countryName>
        public string Execute(string[] data)
        {
            string townName = data[0];
            string countryName = data[1];

            if (!SecurityService.IsAuthenticated())
            {
                throw new InvalidOperationException("Login in order to add tag!");
            }

            if (this.townService.IsTownExisting(townName))
            {
                throw new ArgumentException($"Town {townName} was already added!");
            }

            this.townService.Add(townName, countryName);

            return $"Town {townName} was added to successfully!";
        }
    }
}
