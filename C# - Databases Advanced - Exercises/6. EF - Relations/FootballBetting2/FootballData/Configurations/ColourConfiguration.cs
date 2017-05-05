namespace FootballData.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using FootballModels;

    class ColourConfiguration : EntityTypeConfiguration<Colour>
    {
        public ColourConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Name).IsRequired();
        }
    }
}
