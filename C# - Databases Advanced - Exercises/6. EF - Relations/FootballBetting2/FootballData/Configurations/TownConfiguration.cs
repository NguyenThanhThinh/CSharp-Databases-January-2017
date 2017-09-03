namespace FootballData.Configurations
{
    using FootballModels;
    using System.Data.Entity.ModelConfiguration;

    internal class TownConfiguration : EntityTypeConfiguration<Town>
    {
        public TownConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Name).IsRequired();
            Property(x => x.CountryId).IsRequired();
        }
    }
}