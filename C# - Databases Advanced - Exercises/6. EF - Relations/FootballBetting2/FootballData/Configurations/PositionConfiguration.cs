namespace FootballData.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using FootballModels;

    class PositionConfiguration : EntityTypeConfiguration<Position>
    {
        public PositionConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.PositionDescription).IsRequired();
        }
    }
}
