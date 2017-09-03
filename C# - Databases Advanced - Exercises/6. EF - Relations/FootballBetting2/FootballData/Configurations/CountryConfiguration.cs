namespace FootballData.Configurations
{
    using FootballModels;
    using System.Data.Entity.ModelConfiguration;

    internal class CountryConfiguration : EntityTypeConfiguration<Country>
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