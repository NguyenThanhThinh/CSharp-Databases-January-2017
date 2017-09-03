namespace FootballData.Configurations
{
    using FootballModels;
    using System.Data.Entity.ModelConfiguration;

    internal class BetGameConfiguration : EntityTypeConfiguration<BetGame>
    {
        public BetGameConfiguration()
        {
            HasKey(x => new { x.GameId, x.BetId });
            Property(x => x.GameId).IsRequired().HasColumnOrder(0);
            Property(x => x.BetId).IsRequired().HasColumnOrder(1);

            HasRequired(x => x.ResultPrediction)
                .WithRequiredDependent(x => x.BetGame)
                .Map(m => m.MapKey("ResultPredictionId"));
        }
    }
}