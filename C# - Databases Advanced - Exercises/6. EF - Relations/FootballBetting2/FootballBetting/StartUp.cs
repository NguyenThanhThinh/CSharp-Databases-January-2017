namespace FootballBettingCore
{
    using FootballData;

    class StartUp
    {
        static void Main()
        {
             var context = new FootballBettingDB();
             context.Database.Initialize(true);
        }
    }
}
