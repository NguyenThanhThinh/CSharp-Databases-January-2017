namespace FootballData.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using FootballModels;

    class TownConfiguration : EntityTypeConfiguration<Town>
    {
        public TownConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Name).IsRequired();
            Property(x => x.CountryId).IsRequired();
        }
    }
}
