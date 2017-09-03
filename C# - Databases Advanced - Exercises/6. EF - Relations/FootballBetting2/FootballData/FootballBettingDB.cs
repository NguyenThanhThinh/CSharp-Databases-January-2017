namespace FootballData
{
    using Configurations;
    using FootballModels;
    using System.Data.Entity;

    public class FootballBettingDB : DbContext
    {
        public FootballBettingDB()
             : base("name=FootballBettingDB")
        {
        }

        public virtual DbSet<Bet> Bets { get; set; }
        public virtual DbSet<BetGame> BetGames { get; set; }
        public virtual DbSet<Colour> Colours { get; set; }
        public virtual DbSet<Competition> Competitions { get; set; }
        public virtual DbSet<CompetitionType> CompetitionTypes { get; set; }
        public virtual DbSet<Continent> Continents { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<PlayerStatistic> PlayerStatistics { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<ResultPrediction> ResultPredictions { get; set; }
        public virtual DbSet<Round> Rounds { get; set; }
        public virtual DbSet<Town> Towns { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BetConfiguration());
            modelBuilder.Configurations.Add(new BetGameConfiguration());
            modelBuilder.Configurations.Add(new ColourConfiguration());
            modelBuilder.Configurations.Add(new CompetitionConfiguration());
            modelBuilder.Configurations.Add(new CompetitionTypeConfiguration());
            modelBuilder.Configurations.Add(new CountryConfiguration());
            modelBuilder.Configurations.Add(new TeamConfiguration());
            modelBuilder.Configurations.Add(new GameConfiguration());
            modelBuilder.Configurations.Add(new PlayerConfiguration());
            modelBuilder.Configurations.Add(new PlayerStatisticConfiguration());
            modelBuilder.Configurations.Add(new PositionConfiguration());
            modelBuilder.Configurations.Add(new RoundConfiguration());
            modelBuilder.Configurations.Add(new TownConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new ResultPredictionConfiguration());
        }
    }
}