namespace FootballData.Configurations
{
    using FootballModels;
    using System.Data.Entity.ModelConfiguration;

    internal class RoundConfiguration : EntityTypeConfiguration<Round>
    {
        public RoundConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Name).IsRequired();
        }
    }
}