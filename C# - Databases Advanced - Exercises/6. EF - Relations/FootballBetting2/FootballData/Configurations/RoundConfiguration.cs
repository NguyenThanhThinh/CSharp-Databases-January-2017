namespace FootballData.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using FootballModels;

    class RoundConfiguration : EntityTypeConfiguration<Round>
    {
        public RoundConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Name).IsRequired();
        }
    }
}
