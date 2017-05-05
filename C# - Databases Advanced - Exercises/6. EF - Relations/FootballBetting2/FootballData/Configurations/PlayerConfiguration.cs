namespace FootballData.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using FootballModels;

    class PlayerConfiguration : EntityTypeConfiguration<Player>
    {
        public PlayerConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Name).IsRequired();
            Property(x => x.PositionId).IsRequired();
            Property(x => x.SquadNumber).IsRequired();
            Property(x => x.TeamId).IsRequired();
            Property(x => x.IsInjured).IsRequired();
        }
    }
}
