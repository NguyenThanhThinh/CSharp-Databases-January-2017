namespace FootballData.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using FootballModels;

    class CountryConfiguration : EntityTypeConfiguration<Country>
    {
        public CountryConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Name).IsRequired();

            HasMany(x => x.Continents)
                .WithMany(x => x.Countries)
                .Map(m => m.ToTable("ContinentsCountries")
                    .MapLeftKey("CountryId")
                    .MapRightKey("ContinentId"));
        }
    }
}
