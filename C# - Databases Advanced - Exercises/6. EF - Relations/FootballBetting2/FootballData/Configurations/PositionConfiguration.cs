namespace FootballData.Configurations
{
    using FootballModels;
    using System.Data.Entity.ModelConfiguration;

    internal class PositionConfiguration : EntityTypeConfiguration<Position>
    {
        public PositionConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.PositionDescription).IsRequired();
        }
    }
}