namespace FootballData.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using FootballModels;

    class ContinentConfiguration : EntityTypeConfiguration<Continent>
    {
        public ContinentConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Name).IsRequired();

            HasMany(x => x.Countries)
                .WithMany(x => x.Continents)
                .Map(m => m.ToTable("ContinentsCountries")
                    .MapLeftKey("ContinentId")
                    .MapRightKey("CountryId"));
        }
    }
}
