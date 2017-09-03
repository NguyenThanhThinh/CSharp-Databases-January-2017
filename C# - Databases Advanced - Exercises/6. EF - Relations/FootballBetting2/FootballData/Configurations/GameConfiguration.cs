namespace FootballData.Configurations
{
    using FootballModels;
    using System.Data.Entity.ModelConfiguration;

    internal class GameConfiguration : EntityTypeConfiguration<Game>
    {
        public GameConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.CompetitionId).IsRequired();
            Property(x => x.RoundId).IsRequired();
            Property(x => x.AwayGoals).IsRequired();
            Property(x => x.HomeGoals).IsRequired();
            Property(x => x.HomeTeamWinBetRate).IsRequired();
            Property(x => x.AwayTeamWinBetRate).IsRequired();
            Property(x => x.DrawGameBetRate).IsRequired();
            Property(x => x.GameDateTime).IsRequired();

            HasRequired(x => x.HomeTeam)
               .WithMany(x => x.HomeGames)
               .HasForeignKey(x => x.HomeTeamId)
               .WillCascadeOnDelete(false);

            HasRequired(x => x.AwayTeam)
                .WithMany(x => x.AwayGames)
                .HasForeignKey(x => x.AwayTeamId)
                .WillCascadeOnDelete(false);
        }
    }
}