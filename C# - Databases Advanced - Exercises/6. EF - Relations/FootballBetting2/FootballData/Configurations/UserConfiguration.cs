namespace FootballData.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using FootballModels;

    class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Balance).IsRequired();
            Property(x => x.Email).IsRequired();
            Property(x => x.FullName).IsRequired();
            Property(x => x.Password).IsRequired();
            Property(x => x.Username).IsRequired();
        }
    }
}
