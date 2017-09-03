namespace FootballData.Configurations
{
    using FootballModels;
    using System.Data.Entity.ModelConfiguration;

    internal class CompetitionTypeConfiguration : EntityTypeConfiguration<CompetitionType>
    {
        public CompetitionTypeConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Type).IsRequired();
        }
    }
}