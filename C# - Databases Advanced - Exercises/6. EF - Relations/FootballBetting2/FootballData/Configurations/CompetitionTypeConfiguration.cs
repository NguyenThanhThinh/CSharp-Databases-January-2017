namespace FootballData.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using FootballModels;

    class CompetitionTypeConfiguration : EntityTypeConfiguration<CompetitionType>
    {
        public CompetitionTypeConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Type).IsRequired();
        }
    }
}
