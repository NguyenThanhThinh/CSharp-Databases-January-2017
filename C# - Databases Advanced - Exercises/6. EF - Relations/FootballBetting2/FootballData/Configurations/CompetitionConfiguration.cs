namespace FootballData.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using FootballModels;

    class CompetitionConfiguration : EntityTypeConfiguration<Competition>
    {
        public CompetitionConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Name).IsRequired();
            Property(x => x.CompetitionTypeId).IsRequired();
        }
    }
}
