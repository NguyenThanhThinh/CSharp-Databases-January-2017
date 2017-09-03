namespace FootballData.Configurations
{
    using FootballModels;
    using System.Data.Entity.ModelConfiguration;

    internal class PlayerStatisticConfiguration : EntityTypeConfiguration<PlayerStatistic>
    {
        public PlayerStatisticConfiguration()
        {
            HasKey(x => new { x.GameId, x.PlayerId });
            Property(x => x.GameId).IsRequired().HasColumnOrder(0);
            Property(x => x.PlayerId).IsRequired().HasColumnOrder(1);
            Property(x => x.PlayedMinutes).IsRequired();
            Property(x => x.PlayerAssists).IsRequired();
            Property(x => x.ScoredGoals).IsRequired();
        }
    }
}