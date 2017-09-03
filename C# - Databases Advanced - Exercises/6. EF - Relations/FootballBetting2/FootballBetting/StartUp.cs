namespace FootballBettingCore
{
    using FootballData;

    internal class StartUp
    {
        private static void Main()
        {
            var context = new FootballBettingDB();
            context.Database.Initialize(true);
        }
    }
}