namespace FootballBetting
{
    internal class StartUp
    {
        private static void Main(string[] args)
        {
            FootballBettingContext context = new FootballBettingContext();
            context.Database.Initialize(true);

            // All the models are in folder Models;
            // Check the diagram in MSSQL Server;
        }
    }
}