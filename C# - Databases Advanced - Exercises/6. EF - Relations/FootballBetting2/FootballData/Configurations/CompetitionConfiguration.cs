namespace FootballData.Configurations
{
    using FootballModels;
    using System.Data.Entity.ModelConfiguration;

    internal class CompetitionConfiguration : EntityTypeConfiguration<Competition>
    {
        public CompetitionConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Name).IsRequired();
            Property(x => x.CompetitionTypeId).IsRequired();
        }
    }
}