namespace FootballData.Configurations
{
    using FootballModels;
    using System.Data.Entity.ModelConfiguration;

    internal class TeamConfiguration : EntityTypeConfiguration<Team>
    {
        public TeamConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Name).IsRequired();
            Property(x => x.Initials).IsRequired();
            Property(x => x.TownId).IsRequired();

            HasRequired(x => x.PrmaryKitColour)
                .WithMany(x => x.PrimaryColorTeams)
                .HasForeignKey(x => x.PrimaryKitColourId)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.SecondaryKitColour)
                .WithMany(x => x.SecondaryColorTeams)
                .HasForeignKey(x => x.SecondaryKitColourId)
                .WillCascadeOnDelete(false);
        }
    }
}